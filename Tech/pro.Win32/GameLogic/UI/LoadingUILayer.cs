
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MatrixEngine.CocoStudio.GUI;
using Thunder.GameLogic.Common;
using MatrixEngine.CocoStudio.Armature;
using Thunder.GameLogic.Gaming;
using MatrixEngine.Math;

namespace Thunder.GameLogic.UI
{
    public class LoadingUILayer : UILayer
    {
        UIWidget loading;

        CCArmature loadingArmature;
        CCAnimation loadingAnimation;

        UILabelAtlas AtlasLabel_value;
        UILabel Label_note;

        string[] tips = new string[10];

        public LoadingUILayer()
        {
            loading = UIReader.GetWidget(ResID.UI_UI_Loading);

            ArmatureResManager.Instance.Add(ResID.UI_UIArmature);
            loadingArmature = new CCArmature("UIArmature");
            loadingAnimation = loadingArmature.GetAnimation();

            UILayout Panel_loading = (UILayout)loading.GetWidget("Panel_loading");
            UIImageView Image_anim = (UIImageView)Panel_loading.GetWidget("Image_anim");
            AtlasLabel_value = (UILabelAtlas)Panel_loading.GetWidget("AtlasLabel_value");
            Label_note = (UILabel)loading.GetWidget("Label_note");

            loadingArmature.Postion = Image_anim.Postion;

            Panel_loading.AddNode(loadingArmature);
            loadingAnimation.Play("loading", true);

            tips[0] = "打不过去，升级下战机吧，硬碰硬可不是过关好方法。";
            tips[1] = "升级VIP可获得大量特权！";
            tips[2] = "关键时刻使用必杀可以容出重围...";
            tips[3] = "战机越高级，战斗力更强！";
            tips[4] = "遇到困难时，可以使用必杀清屏！";
            tips[5] = "多预备些护盾，遇到再强的BOSS也无所畏惧！";
            tips[6] = "VIP用户过关会有大量额外奖励！";
            tips[7] = "后期还有更多酷炫的关卡和BOSS，敬请关注哦！";
            tips[8] = "BOSS弹幕存在弱点，注意观察可轻松击败它们！";
            tips[9] = "购买限时礼包，一秒变土豪！";

            this.AddWidget(loading);
        }

        public override void OnEnter()
        {
            loadingAnimation.Play("loading", true);
            AtlasLabel_value.Text = "0";
            Label_note.Text = tips[MathHelper.Random_minus0_n(tips.Length - 1)];

            GameAudio.PlayEffect(GameAudio.Effect.loading,false);
            GameAudio.StopMusic();
        }

        public override void OnExit()
        {
            base.OnExit();

            GameAudio.StopEffect(GameAudio.Effect.loading);
        }

        public override void OnEnterTransitionFinish()
        {
            base.OnEnterTransitionFinish();
        }

        public override void OnExitTransitionStart()
        {
            base.OnExitTransitionStart();
        }

        public void SetLoadingValue(int value)
        {
            AtlasLabel_value.Text = Convert.ToString(value);
        }

    }
}
