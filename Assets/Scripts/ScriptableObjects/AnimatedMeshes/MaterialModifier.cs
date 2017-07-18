using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AnimationFX/MaterialModifier/Standard")]
public class MaterialModifier :             ScriptableObject
{
    public MaterialModifierData             data;

    public void                             update(MaterialModifierState state, float ratio)
    {
        state.tiling.x = Mathf.Lerp(this.data.tilingBegin.x, this.data.tilingEnd.x, this.data.xTilingCurve.Evaluate(ratio));
        state.tiling.y = Mathf.Lerp(this.data.tilingBegin.y, this.data.tilingEnd.y, this.data.yTilingCurve.Evaluate(ratio));

        state.offset.x = Mathf.Lerp(this.data.offsetBegin.x, this.data.offsetEnd.x, this.data.xOffsetCurve.Evaluate(ratio));
        state.offset.y = Mathf.Lerp(this.data.offsetBegin.y, this.data.offsetEnd.y, this.data.yOffsetCurve.Evaluate(ratio));

        state.tint = Color.Lerp(this.data.tintBegin, this.data.tintEnd, this.data.tintCurve.Evaluate(ratio));
    }

    public void                             updateMaterial(MaterialModifierState state, Material mat)
    {
        mat.SetTextureOffset("_MainTex", state.offset);
        mat.SetTextureScale("_MainTex", state.tiling);
        mat.SetColor("_TintColor", state.tint);
    }
}
