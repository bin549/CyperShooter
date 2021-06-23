using UnityEngine;
using UnityEngine.AI;

public class FootstepListener : MonoBehaviour
{
    public FootstepAudioData FootstepAudioData;
    public AudioSource FootstepAudioSource;
    private Transform FootstepTransform;
    //private Vector3 LastFootstepTransformPosition;
    private float nextPlayTime;
    public NavMeshAgent NavMeshAgent;

    private void Start()
    {
        FootstepTransform = transform;
        // LastFootstepTransformPosition = FootstepTransform.position;
    }

    private void FixedUpdate()
    {
        if (NavMeshAgent.desiredVelocity.magnitude != 0)
        {
            nextPlayTime += Time.fixedDeltaTime;

            if (Physics.Raycast(FootstepTransform.position, Vector3.down, out RaycastHit tmp_hit, 0.5f))
            {
                foreach (var tmp_AudioElement in FootstepAudioData.FootstepAudios)
                {
                    float tmp_Delay = 0;
                    tmp_Delay = tmp_AudioElement.Delay;
                    if (tmp_hit.collider.CompareTag(tmp_AudioElement.Tag))
                    {
                        if (nextPlayTime >= tmp_Delay)
                        {
                            int tmp_AudioCount = tmp_AudioElement.AduioClips.Count;
                            int tmp_AudioIndex = UnityEngine.Random.Range(0, tmp_AudioCount);
                            AudioClip tmp_FootsetpAudioClip = tmp_AudioElement.AduioClips[tmp_AudioIndex];
                            FootstepAudioSource.clip = tmp_FootsetpAudioClip;
                            FootstepAudioSource.Play();
                            nextPlayTime = 0;
                            break;
                        }
                    }
                }
            }
        }
    }
}
