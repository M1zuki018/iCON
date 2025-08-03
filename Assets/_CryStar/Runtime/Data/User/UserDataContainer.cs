using CryStar.Field.Data;

namespace CryStar.Data
{
    public class UserDataContainer
    {
        private FieldSaveData _fieldSaveData;
        private StoryUserData _storyUserData;
    
        public FieldSaveData FieldSaveData => _fieldSaveData;
        public StoryUserData StoryUserData => _storyUserData;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserDataContainer(int userId)
        {
            _fieldSaveData = new FieldSaveData(userId);
            _storyUserData = new StoryUserData(userId);
        }

        /// <summary>
        /// 全データの保存時刻を更新
        /// </summary>
        public void UpdateAllSaveTimes()
        {
            _fieldSaveData?.UpdateSaveTime();
            _storyUserData?.UpdateSaveTime();
        }
    }
}