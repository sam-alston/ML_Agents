using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.Video;

public class HungryBlockScript : Agent
{
    public GameObject target;
    Rigidbody m_HungryBlockRb;
    public float m_speed = 8.0f;
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        m_HungryBlockRb = GetComponent<Rigidbody>();
        //m_HungryBlockRb.constraints = RigidbodyConstraints.FreezePositionY;
        //m_HungryBlockRb.constraints = RigidbodyConstraints.FreezeRotationX;
        //m_HungryBlockRb.constraints = RigidbodyConstraints.FreezeRotationZ;
        this.transform.localPosition = new Vector3(Random.Range(1f, 17f), -2.5f, Random.Range(-2f, -3f));
        //this.transform.localPosition = new Vector3(9f, -2.5f, -10f);

        this.transform.eulerAngles = new Vector3(0f, Random.Range(150f, 210f), 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //direction to target
        sensor.AddObservation((gameObject.transform.position - target.transform.position).normalized);
        //distance to target
        sensor.AddObservation(UnityEngine.Vector3.Distance(gameObject.transform.position,target.transform.position));
        //Block facing
        sensor.AddObservation(gameObject.transform.forward);
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        //var actionY = Mathf.Clamp(vectorAction[0], -1f, 1f);
        //var actionX = Mathf.Clamp(vectorAction[1], -1f, 1f);
        var actionY = vectorAction[0];
        var actionX = vectorAction[1];


        
        //gameObject.transform.Rotate(new Vector3(0, 1f, 0), actionY * 4);
        if (actionY == 2)
        {
            //rotate in Y right
            gameObject.transform.Rotate(new UnityEngine.Vector3(0, -1f, 0), actionY * 4);
        } else
        {
            //rotate right or nothing
            gameObject.transform.Rotate(new UnityEngine.Vector3(0, 1f, 0), actionY * 4);
        }

        //movement
        if(actionX == 2)
        {
            //forward
            m_HungryBlockRb.velocity = transform.forward * m_speed * actionX;
        } else
        {
            //stop/back
            m_HungryBlockRb.velocity = transform.forward * m_speed * actionX * -1f;
        }

        //end when flying off map
        if (gameObject.transform.localPosition.y > 1f)
        {
            SetReward(-50f);
            EndEpisode();
        }

        if (Mathf.Abs(target.transform.localPosition.x - gameObject.transform.localPosition.x) < 0.5f &&
            Mathf.Abs(target.transform.localPosition.z - gameObject.transform.localPosition.z) < 0.5f)
        {
            SetReward(0.1f);
            EndEpisode();
        }
        else
        {
            SetReward(-1f);
        }
    }

    public override void OnEpisodeBegin()
    {
        //this.transform.localPosition = new Vector3(Random.Range(1f,17f), -2.5f, Random.Range(-2f,-3f));
        ////this.transform.localPosition = new Vector3(9f, -2.5f, -10f);

        //this.transform.eulerAngles = new Vector3(0f, Random.Range(150f,210f), 0f);
        target.transform.localPosition = new Vector3(Random.Range(2f, 17f), -2f, Random.Range(-3.5f, -18f));
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }
}
