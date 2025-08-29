using CryStar.Data.User;
using CryStar.Field.Data;

namespace CryStar.Data
{
    public class UserDataContainer
    {
        private FieldSaveData _fieldSaveData;
        private StoryUserData _storyUserData;
        private CharacterUserData _characterUserData;
        private GameEventUserData _gameEventUserData;
    
        public FieldSaveData FieldSaveData => _fieldSaveData;
        public StoryUserData StoryUserData => _storyUserData;
        public CharacterUserData CharacterUserData => _characterUserData;
        public GameEventUserData GameEventUserData => _gameEventUserData;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserDataContainer(int userId)
        {
            _fieldSaveData = new FieldSaveData(userId);
            _storyUserData = new StoryUserData(userId);
            _characterUserData = new CharacterUserData(userId);
            _gameEventUserData = new GameEventUserData(userId);
        }

        /// <summary>
        /// 全データの保存時刻を更新
        /// </summary>
        public void UpdateAllSaveTimes()
        {
            _fieldSaveData?.UpdateSaveTime();
            _storyUserData?.UpdateSaveTime();
            _characterUserData?.UpdateSaveTime();
            _gameEventUserData?.UpdateSaveTime();
        }
    }
}