using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OP2MoveDoor : MonoBehaviour
{
    //int x_axis = 0;
    float cur_x = 0;
    float cur_y = 0;
    float cur_z = 0;
    //float speed = 50f;
    int position_way = 0; //0不動，1開門，2關門
    float movingSpeed = 500f;
    // Start is called before the first frame update
    void Start()
    {
        cur_x = this.transform.position.x;
        cur_y = this.transform.position.y;
        cur_z = this.transform.position.z;
        position_way = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(position_way == 1){
            if(this.transform.localPosition.x < 1000){
                this.transform.localPosition  = this.transform.localPosition  + new Vector3(1*movingSpeed, 0*movingSpeed, 0*movingSpeed) * Time.deltaTime;
            }
            else{
                position_way = 2;
            }
        } else if(position_way == 2){
            if(this.transform.localPosition.x > 0){
                this.transform.localPosition  = this.transform.localPosition  + new Vector3(-1*movingSpeed, 0*movingSpeed, 0*movingSpeed) * Time.deltaTime;
            }
            else{
                position_way = 1;
            }
        }

        GameObject.Find("ButtonOpenDoor").GetComponentInChildren<Text>().text = this.transform.localPosition.x.ToString();

       /* cur_x = this.transform.position.x;
        cur_y = this.transform.position.y;
        cur_z = this.transform.position.z;
        if(position_way == 1){ //如果是開門
            while(cur_x <= 1000){
                transform.position = new Vector3(100,0,0) * Time.deltaTime;
            }
            position_way = 2;
        }else if(position_way == 2){ //如果是開門
            while(cur_x >= 0){
                transform.position = new Vector3(-100,0,0) * Time.deltaTime;
            }
            position_way = 1;
        }*/
        
        //transform
        //tra
    }
}
