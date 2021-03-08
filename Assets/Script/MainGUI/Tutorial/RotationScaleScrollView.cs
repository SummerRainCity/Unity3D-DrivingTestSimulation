using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScaleScrollView : ScalePageScrollView
{

    public float rotation;

    protected override void Update()
    {
        base.Update();
        ListenerItemRotation();

    }

    public void ListenerItemRotation() {


        if (nextPage == lastPage)
        {
            return;
        }

        float percent = (rect.horizontalNormalizedPosition - pages[lastPage]) / (pages[nextPage] - pages[lastPage]);

        if (nextPage > currentPage)
        {
            items[lastPage].transform.localRotation = Quaternion.Euler(-Vector3.Lerp(Vector3.zero, new Vector3(0, rotation, 0), percent));
            items[nextPage].transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, rotation, 0), 1 - percent));
        }
        else {
            items[lastPage].transform.localRotation = Quaternion.Euler(-Vector3.Lerp(Vector3.zero, new Vector3(0, rotation, 0), percent));
            items[nextPage].transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, rotation, 0), 1 - percent));
        }

       


        for (int i = 0; i < items.Length; i++)
        {
            //if (i != lastPage && i != nextPage)
            //{
            //    items[i].transform.localScale = Vector3.one * otherScale;
            //}

            if ( i != lastPage && i != nextPage )
            {
                if (i < currentPage)
                {
                    items[i].transform.localRotation = Quaternion.Euler(new Vector3(0, -rotation, 0));
                }
                else if (i == currentPage)
                {
                    //items[i].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                else if (i > currentPage)
                {
                    items[i].transform.localRotation = Quaternion.Euler(new Vector3(0, rotation, 0));
                }
            }
        }
    }

}
