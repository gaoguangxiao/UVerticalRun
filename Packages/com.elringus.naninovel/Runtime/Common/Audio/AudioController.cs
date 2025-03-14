using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Naninovel
{
    /// <summary>
    /// Manages <see cref="AudioTrack"/> objects.
    /// </summary>
    public class AudioController : MonoBehaviour
    {
        public AudioListener Listener => listenerCache ? listenerCache : FindOrAddListener();
        public float Volume { get => AudioListener.volume; set => AudioListener.volume = value; }
        public bool Mute { get => AudioListener.pause; set => AudioListener.pause = value; }

        private readonly List<AudioTrack> audioTracks = new();
        private readonly Stack<AudioSource> sourcesPool = new();
        private AudioListener listenerCache;
        private Tweener<FloatTween> listenerVolumeTweener;

        private void Awake ()
        {
            listenerVolumeTweener = new();
            FindOrAddListener();
        }

        /// <summary>
        /// Sets transform of the current <see cref="Listener"/> as a child of the specified target.
        /// </summary>
        public void AttachListener (Transform target)
        {
            Listener.transform.SetParent(target);
            Listener.transform.localPosition = Vector3.zero;
        }

        public void FadeVolume (float volume, float time)
        {
            if (listenerVolumeTweener.Running)
                listenerVolumeTweener.CompleteInstantly();

            var tween = new FloatTween(Volume, volume, new(time, scale: false), value => Volume = value);
            listenerVolumeTweener.Run(tween, target: this);
        }

        public bool ClipPlaying (AudioClip clip)
        {
            if (!clip) return false;
            foreach (var track in audioTracks)
                if (track.Clip == clip && track.Playing)
                    return true;
            return false;
        }

        public void PlayClip (AudioClip clip, AudioSource audioSource = null, float volume = 1f,
            bool loop = false, AudioMixerGroup mixerGroup = null, AudioClip introClip = null, bool additive = false)
        {
            if (!clip) return;

            if (!additive) StopClip(clip);
            PoolUnusedSources();

            // In case user somehow specified one of our pooled sources, don't use it.
            if (audioSource && IsOwnedByController(audioSource)) audioSource = null;
            if (!audioSource) audioSource = GetPooledSource();

            var track = new AudioTrack(clip, audioSource, volume, loop, mixerGroup, introClip);
            audioTracks.Add(track);
            track.Play();
        }

        public async UniTask PlayClip (AudioClip clip, float fadeInTime, AudioSource audioSource = null, float volume = 1f,
            bool loop = false, AudioMixerGroup mixerGroup = null, AudioClip introClip = null, bool additive = false, AsyncToken token = default)
        {
            if (!clip) return;

            if (!additive) StopClip(clip);
            PoolUnusedSources();

            // In case user somehow specified one of our pooled sources, don't use it.
            if (audioSource && IsOwnedByController(audioSource)) audioSource = null;
            if (!audioSource) audioSource = GetPooledSource();

            var track = new AudioTrack(clip, audioSource, volume, loop, mixerGroup, introClip);
            audioTracks.Add(track);
            await track.Play(fadeInTime, token);
        }

        public void StopClip (AudioClip clip)
        {
            if (!clip || !ClipPlaying(clip)) return;
            foreach (var track in GetTracks(clip))
                track.Stop();
        }

        public void StopAllClips ()
        {
            foreach (var track in GetAllTracks())
                track.Stop();
        }

        public async UniTask StopClip (AudioClip clip, float fadeOutTime, AsyncToken token = default)
        {
            if (!clip || !ClipPlaying(clip)) return;
            using var _ = ListPool<UniTask>.Rent(out var tasks);
            foreach (var track in GetTracks(clip))
                tasks.Add(track.Stop(fadeOutTime, token));
            await UniTask.WhenAll(tasks);
        }

        public async UniTask StopAllClips (float fadeOutTime, AsyncToken token = default)
        {
            using var _ = ListPool<UniTask>.Rent(out var tasks);
            foreach (var track in GetAllTracks())
                tasks.Add(track.Stop(fadeOutTime, token));
            await UniTask.WhenAll(tasks);
        }

        public IReadOnlyCollection<AudioTrack> GetTracks (AudioClip clip)
        {
            if (!clip) return null;
            return audioTracks.Where(t => t.Clip == clip).ToArray();
        }

        public IReadOnlyCollection<AudioTrack> GetAllTracks ()
        {
            return audioTracks.ToArray();
        }

        private AudioListener FindOrAddListener ()
        {
            listenerCache = FindFirstObjectByType<AudioListener>();
            if (!listenerCache) listenerCache = gameObject.AddComponent<AudioListener>();
            return listenerCache;
        }

        private bool IsOwnedByController (AudioSource audioSource)
        {
            return audioSource && audioSource.gameObject == gameObject;
        }

        private AudioSource GetPooledSource ()
        {
            if (sourcesPool.Count > 0) return sourcesPool.Pop();
            return gameObject.AddComponent<AudioSource>();
        }

        private void PoolUnusedSources ()
        {
            for (int i = audioTracks.Count - 1; i >= 0; i--)
            {
                var track = audioTracks[i];
                if (track.Playing) continue;
                if (IsOwnedByController(track.Source))
                    sourcesPool.Push(track.Source);
                audioTracks.Remove(track);
            }
        }
    }
}
