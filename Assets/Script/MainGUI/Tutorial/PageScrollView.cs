using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PageScrollType {
    Horizontal,
    Vertical
}

public class PageScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{

    #region 字段

    protected ScrollRect rect;
    protected int pageCount;
    private RectTransform content;
    protected float[] pages;

    public float moveTime = 0.3f;
    private float timer = 0;
    private float startMovePos;
    protected int currentPage = 0;

    private bool isDraging = false;

    private bool isMoving = false;
    // 是不是开启自动滚动
    public bool IsAutoScroll;
    public float AutoScrollTime = 2;
    private float AutoScrollTimer = 0;

    public PageScrollType pageScrollType = PageScrollType.Horizontal;

    public float vertical;

    #endregion

    #region 事件

    public Action<int> OnPageChange;

    #endregion

    #region Unity回调
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ListenerMove();
        ListenerAutoScroll();
        vertical = rect.verticalNormalizedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 滚动到离得最近的一页
        this.ScrollToPage(CaculateMinDistancePage());
        isDraging = false;
        AutoScrollTimer = 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;
    }

    #endregion


    #region 方法 

    public void Init() {
        rect = transform.GetComponent<ScrollRect>();
        if (rect == null)
        {
            throw new System.Exception(" 未查询到ScrollRect! ");
        }
        content = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        pageCount = content.childCount;
        if (pageCount == 1)
        {
            throw new System.Exception("只有一页，不用进行分页滚动!");
        }
        pages = new float[pageCount];
        for (int i = 0; i < pages.Length; i++)
        {
            switch (pageScrollType)
            {
                case PageScrollType.Horizontal:
                    pages[i] = i * (1.0f / (float)(pageCount - 1));
                    break;
                case PageScrollType.Vertical:
                    pages[i] = 1 - i * (1.0f / (float)(pageCount - 1));
                    break;
            }

        }

    }

    // 监听移动
    public void ListenerMove()
    {
        if (isMoving)
        {
            timer += Time.deltaTime * (1 / moveTime);
            switch (pageScrollType)
            {
                case PageScrollType.Horizontal:
                    rect.horizontalNormalizedPosition = Mathf.Lerp(startMovePos, pages[currentPage], timer);
                    break;
                case PageScrollType.Vertical:
                    rect.verticalNormalizedPosition = Mathf.Lerp(startMovePos, pages[currentPage], timer);
                    break;
            }
            if (timer >= 1)
            {
                isMoving = false;
            }
        }
    }
    // 监听自动滚动
    public void ListenerAutoScroll()
    {

        if (isDraging) { return; }

        if (IsAutoScroll)
        {
            AutoScrollTimer += Time.deltaTime;
            if (AutoScrollTimer >= AutoScrollTime)
            {
                AutoScrollTimer = 0;
                // 滚动到下一页
                currentPage++;
                currentPage %= pageCount;
                ScrollToPage(currentPage);
            }
        }
    }

    public void ScrollToPage(int page)
    {
        isMoving = true;
        this.currentPage = page;
        timer = 0;
        switch (pageScrollType)
        {
            case PageScrollType.Horizontal:
                startMovePos = rect.horizontalNormalizedPosition;
                break;
            case PageScrollType.Vertical:
                startMovePos = rect.verticalNormalizedPosition;
                break;
        }
       

        if (OnPageChange != null)
        {
            OnPageChange(this.currentPage);
        }
    }

    // 计算离得最近的一页
    public int CaculateMinDistancePage() {
        int minPage = 0;
        // 计算出离得最近的一页
        for (int i = 1; i < pages.Length; i++)
        {
            switch (pageScrollType)
            {
                case PageScrollType.Horizontal:

                    if (Mathf.Abs(pages[i] - rect.horizontalNormalizedPosition) < Mathf.Abs(pages[minPage] - rect.horizontalNormalizedPosition))
                    {
                        minPage = i;
                    }
                    break;
                case PageScrollType.Vertical:

                    if (Mathf.Abs(pages[i] - rect.verticalNormalizedPosition) < Mathf.Abs(pages[minPage] - rect.verticalNormalizedPosition))
                    {
                        minPage = i;
                    }
                    break;
            }



        }
        return minPage;
    }
    
    #endregion

}
