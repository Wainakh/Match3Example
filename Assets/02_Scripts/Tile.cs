using UnityEngine;

namespace M3T
{
    /// <summary>
    /// Базовый класс фишки на поле
    /// </summary>
    public class Tile : MonoBehaviour
    {
        public Vector2Int position;
        public TileData tileData;
        [SerializeField] GameObject ExpodeVFX;
        public Vector3 DefaultPosition { get; private set; }

        public SpriteRenderer sprite;
        public GameObject predictionSelected;
        public GameObject userSelected;

        public bool isInited = false;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (isInited)
                return;
            DefaultPosition = transform.localPosition;
            sprite = transform.Find("Render").GetComponent<SpriteRenderer>();
            predictionSelected = transform.Find("PredictionSelected").gameObject;
            userSelected = transform.Find("UserSelected").gameObject;
            isInited = true;
            SetPredictionSelection(false);
            SetUserSelection(false);
        }

        public Vector2 GetSize()
        {
            if (sprite == null)
                sprite = transform.Find("Render").GetComponent<SpriteRenderer>();
            return sprite.bounds.size;
        }

        public void SetUserSelection(bool value)
        {
            Initialize();
            userSelected.SetActive(value);
        }

        public void SetPredictionSelection(bool value)
        {
            Initialize();
            predictionSelected.SetActive(value);
        }

        public void ApplyNewData(TileData curTileData)
        {
            tileData = curTileData;
            sprite.color = tileData.IsNullOrDefault() ? default : tileData.color;
            if (!tileData.IsNullOrDefault() && !tileData.sprite.IsNullOrDefault())
                sprite.sprite = tileData.sprite;
        }

        #region Временный костыль ввиду отсутсвия UserInput
        //Временный костыль ввиду отсутсвия UserInput
        private void OnMouseDown()
        {
            GameController.Instance.OnTileClick(this);
        }
        #endregion

        internal void Explode()
        {
            if (ExpodeVFX != null)
                Instantiate(ExpodeVFX, transform.position, Quaternion.identity);
            ApplyNewData(null);
        }

        internal void Select()
        {
            SetUserSelection(true);
        }

        internal void Deselect()
        {
            SetUserSelection(false);
        }
    }
}
