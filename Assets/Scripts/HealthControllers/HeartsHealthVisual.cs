using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkPath
{
    public class HeartsHealthVisual : MonoBehaviour
    {
        public Sprite[] heart;

        private List<HeartImage> heartImageList;
        private HeartsHealthSystem heartsHealthSystem;

        private void Awake()
        {
            heartImageList = new List<HeartImage>();
        }

        public void SetHeartsHealthSystem(HeartsHealthSystem heartsHealthSystem)
        {
            this.heartsHealthSystem = heartsHealthSystem;

            List<HeartsHealthSystem.Heart> heartList = heartsHealthSystem.GetHeartList();
            int row = 0;
            int col = 0;
            int colMax = 10;
            float rowColSize = 30f;

            for (int i = 0; i < heartList.Count; i++)
            {
                HeartsHealthSystem.Heart heart = heartList[i];
                Vector2 heartAnchorPosition = new Vector2(col * rowColSize, -row * rowColSize);
                CreateHeartImage(heartAnchorPosition).SetHeartFragment(heart.GetFragmentsAmount());

                col++;
                if (col >= colMax)
                {
                    row++;
                    col = 0;
                }
            }
        }

        public void RefreshAllHearts()
        {
            List<HeartsHealthSystem.Heart> heartList = heartsHealthSystem.GetHeartList();
            for (int i = 0; i < heartImageList.Count; i++)
            {
                HeartImage heartImage = heartImageList[i];
                HeartsHealthSystem.Heart heart = heartList[i];
                heartImage.SetHeartFragment(heart.GetFragmentsAmount());
            }
        }

        private HeartImage CreateHeartImage(Vector2 anchoredPosition)
        {
            GameObject heartGameObject = new GameObject("Heart", typeof(Image));
            // Set Child
            heartGameObject.transform.SetParent(transform);
            heartGameObject.transform.localPosition = Vector3.zero;

            // Place and Size 
            heartGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
            heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);

            // Set Sprite
            Image heartImageUI = heartGameObject.GetComponent<Image>();
            heartImageUI.sprite = heart[0];

            HeartImage heartImage = new HeartImage(this, heartImageUI);
            heartImageList.Add(heartImage);

            return heartImage;
        }

        // A single heart
        public class HeartImage
        {
            private Image heartImage;
            private HeartsHealthVisual heartsHealthVisual;

            public HeartImage(HeartsHealthVisual heartsHealthVisual, Image heartImage)
            {
                this.heartsHealthVisual = heartsHealthVisual;
                this.heartImage = heartImage;
            }

            public void SetHeartFragment(int fragments)
            {
                switch (fragments)
                {
                    case 0:
                        heartImage.sprite = heartsHealthVisual.heart[0];
                        break;
                    case 1:
                        heartImage.sprite = heartsHealthVisual.heart[1];
                        break;
                    case 2:
                        heartImage.sprite = heartsHealthVisual.heart[2];
                        break;
                    case 3:
                        heartImage.sprite = heartsHealthVisual.heart[3];
                        break;
                    case 4:
                        heartImage.sprite = heartsHealthVisual.heart[4];
                        break;
                    default:
                        Debug.LogWarning("Exceeded maximum heart segments!");
                        heartImage.sprite = heartsHealthVisual.heart[0];
                        break;
                }
            }
        }
    }
}