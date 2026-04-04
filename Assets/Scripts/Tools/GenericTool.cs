namespace BoatGame
{
    public class GenericTool : Tool
    {
        public override void OnActivate()
        {
            gameObject.SetActive(true);
        }

        public override void OnDeactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
