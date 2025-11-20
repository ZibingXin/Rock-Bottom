using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionUI : MonoBehaviour
{
    [System.Serializable]
    public class Page
    {
        public string title;
        [TextArea]
        public string content;
    }

    public Page[] pages;

    public TMP_Text titleText;
    public TMP_Text contentText;
    public Button prevButton;
    public Button nextButton;

    private int currentPage = 0;

    void Start()
    {
        ShowPage(0);

        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
    }

    void ShowPage(int index)
    {
        currentPage = Mathf.Clamp(index, 0, pages.Length - 1);

        titleText.text = pages[currentPage].title;
        contentText.text = pages[currentPage].content;

        prevButton.interactable = currentPage > 0;
        nextButton.interactable = currentPage < pages.Length - 1;
    }

    void PrevPage()
    {
        ShowPage(currentPage - 1);
    }

    void NextPage()
    {
        ShowPage(currentPage + 1);
    }
}
