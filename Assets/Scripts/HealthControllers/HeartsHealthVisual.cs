using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkPath
{
    public class HeartsHealthVisual : MonoBehaviour
    {
        [SerializeField]
        private Sprite heart0Sprite = null;
        [SerializeField]
        private Sprite heart1Sprite = null;
        [SerializeField]
        private Sprite heart2Sprite = null;
        [SerializeField]
        private Sprite heart3Sprite = null;
        [SerializeField]
        private Sprite heart4Sprite = null;

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
            heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);

            // Set Sprite
            Image heartImageUI = heartGameObject.GetComponent<Image>();
            heartImageUI.sprite = heart0Sprite;

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
                        heartImage.sprite = heartsHealthVisual.heart0Sprite;
                        break;
                    case 1:
                        heartImage.sprite = heartsHealthVisual.heart1Sprite;
                        break;
                    case 2:
                        heartImage.sprite = heartsHealthVisual.heart2Sprite;
                        break;
                    case 3:
                        heartImage.sprite = heartsHealthVisual.heart3Sprite;
                        break;
                    case 4:
                        heartImage.sprite = heartsHealthVisual.heart4Sprite;
                        break;
                    default:
                        Debug.LogWarning("Exceeded maximum heart segments!");
                        heartImage.sprite = heartsHealthVisual.heart0Sprite;
                        break;
                }
            }
        }
    }
}