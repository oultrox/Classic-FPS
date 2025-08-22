using UnityEngine;
using System.Collections;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed = 0.18f;
    [SerializeField] private float bobbingHeight = 0.2f;
    [SerializeField] private float midpoint = 1.8f;
    [SerializeField] private bool isHeadBobbing = true;
    private float timer = 0.0f;

    private float translateChange;
    private float totalAxes;
    private Vector3 cSharpConversion;
    private float limitIteration = Mathf.PI * 2;
    
    void Update()
    {
        AdjustBobbling();
    }

    private void AdjustBobbling()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        cSharpConversion = transform.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            
            // Timer routine for making the bobbling.
            timer = timer + (bobbingSpeed * Time.deltaTime);
            if (timer > limitIteration)
            {
                timer = timer - limitIteration;
            }
        }

        if (waveslice != 0)
        {
            translateChange = waveslice * bobbingHeight;
            totalAxes = (Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            if (isHeadBobbing)
                cSharpConversion.y = midpoint + translateChange;
            else
                cSharpConversion.x = translateChange;
        }
        else
        {
            if (isHeadBobbing)
                cSharpConversion.y = midpoint;
            else
                cSharpConversion.x = 0;
        }
        transform.localPosition = cSharpConversion;
    }
}