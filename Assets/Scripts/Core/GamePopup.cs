using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public class GamePopup : MonoBehaviour
    {
        public static int NumberPopupShow => currentPopups.Count;
        public static bool IsHavePopupOpen => currentPopups.Count != 0;

        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();

                return rectTransform;
            }
        }

        public static GamePopup CurrentPopup => currentPopups.Count != 0 ? currentPopups[0] : null;

        [SerializeField] protected Vector4 Margin = new(10, 10, 10, 20);
        [SerializeField] protected Animator animatorController;

        protected static List<GamePopup> currentPopups = new();

        protected RectTransform rectTransform;
        protected UnityAction<object> closeCallback;
        protected bool isShowBackgroundPopup;

        private const string AnimationShow = "Gamepopup Show";
        private const string AnimationParameter_Close = "Close";

        protected static bool LockShowPopup;

        public delegate bool CheckCondition();
        protected CheckCondition condition;


        #region Virtual Method
        public virtual void Show(bool isHideLastPopup = true, object data = null, UnityAction<object> closeCallback = null, bool isShowBackgroundPopup = true)
        {
            if (!LockShowPopup || (condition != null && condition()))
            {
                if (currentPopups.Count != 0)
                {
                    if (currentPopups[0] == null)
                    {
                        currentPopups.RemoveAt(0);
                    }
                    else
                    {
                        if (currentPopups[0] == this) return;

                        currentPopups[0].gameObject.SetActive(!isHideLastPopup);
                    }
                }

                gameObject.SetActive(true);
                this.isShowBackgroundPopup = isShowBackgroundPopup;

                if (animatorController != null)
                    animatorController.Play(AnimationShow, -1, 0);

                if (isShowBackgroundPopup)
                {
                    //UiManager.Instance.ShowBackgroundPopup(RectTransform, Margin);
                }
                else
                {
                    //UiManager.Instance.HideBackgroundPopup();
                }

                transform.SetAsLastSibling();
                currentPopups.Insert(0, this);
            }
            else
            {
                gameObject.SetActive(false);
            }

            this.closeCallback = closeCallback;
        }

        protected virtual void Hide(object dataSendBack = null)
        {
            gameObject.SetActive(false);

            if (currentPopups.Count != 0)
            {
                if (currentPopups[0] == this)
                {
                    // close current popup
                    if (isShowBackgroundPopup)
                        //UiManager.Instance.HideBackgroundPopup();

                    currentPopups.RemoveAt(0);
                    closeCallback?.Invoke(dataSendBack);


                    // show popup previous
                    if (currentPopups.Count != 0)
                    {
                        bool isPlayAnimShow = !currentPopups[0].gameObject.activeInHierarchy;

                        if (isPlayAnimShow)
                        {
                            currentPopups[0].gameObject.SetActive(true);

                            if (currentPopups[0].animatorController != null)
                            {
                                currentPopups[0].animatorController.Play(AnimationShow, -1, 0);
                            }

                            currentPopups[0].Reshow(dataSendBack);
                        }

                        if (currentPopups[0].isShowBackgroundPopup)
                        {
                            //UiManager.Instance.ShowBackgroundPopup(currentPopups[0].RectTransform, currentPopups[0].Margin);
                            currentPopups[0].transform.SetAsLastSibling();
                        }
                    }
                }
                else
                {
                    currentPopups.Remove(this);
                }
            }
            else
            {
                //UiManager.Instance.HideBackgroundPopup();
            }
        }

        protected virtual void Reshow(object dataSendBack = null) { }

        
        public virtual void Click_BackgroundPopup() { }
        #endregion

        public void Click_DefaultClosePopup()
        {
            //Logger.Log("Click_DefaultClosePopup");
            //SoundManager.Instance?.PlayOneShotSound(SoundType.click_button);
            DefaultClosePopup();
        }

        protected void ClosePopup()
        {
            DefaultClosePopup(false);
        }

        public virtual void DefaultClosePopup(bool usingAnimation = true)
        {
            if (usingAnimation && animatorController != null)
            {
                animatorController.SetTrigger(AnimationParameter_Close);
            }
            else
            {
                Hide();
            }
        }

        private void AnimationCall_Hide(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 1f)
            {
                Hide();
            }
        }

        public static void ClearAllPopup()
        {
            foreach (var popup in currentPopups)
            {
                if (popup != null)
                {
                    popup.gameObject.SetActive(false);
                }
            }
            currentPopups.Clear();
        }

        public static void ClearAllPopupExcept<T>() where T : GamePopup
        {
            for (int i = 0; i < currentPopups.Count;)
            {
                if (currentPopups[i] as T)
                {
                    currentPopups[i].gameObject.SetActive(true);
                    i++;
                    continue;
                }
                else
                {
                    currentPopups[i].gameObject.SetActive(false);
                    currentPopups.RemoveAt(i);
                }
            }
        }
    }

}
