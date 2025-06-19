using UnityEngine;

public class BackgroundLogic 
{
    SpriteRenderer firstBackGround;
    SpriteRenderer secondBackGround;
    SpriteRenderer thirdBackGround;

    private float parallaxFactor1 = 0.15f;
    private float parallaxFactor2 = 0.2f;
    private float parallaxFactor3 = 0.1f;

    public BackgroundLogic(AdressableInstantiator adressable, SpriteRenderer firstRenderer, SpriteRenderer secondRenderer, SpriteRenderer thirdRenderer)
    {
        firstBackGround = firstRenderer;
        secondBackGround = secondRenderer;
        thirdBackGround = thirdRenderer;

        firstBackGround.sprite = adressable.GetInstanceSprite("background1");
        secondBackGround.sprite = adressable.GetInstanceSprite("background2");
        thirdBackGround.sprite = adressable.GetInstanceSprite("background3");
    }

    public void ParallaxEffect(float deltaX)
    {
        firstBackGround.transform.position = new Vector3(deltaX * parallaxFactor1, 0f, 0f);
        secondBackGround.transform.position += new Vector3(deltaX * parallaxFactor2, 0f, 0f);
        thirdBackGround.transform.position += new Vector3(deltaX * parallaxFactor3, 0f, 0f);
    }
}
