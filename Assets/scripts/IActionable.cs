using System.Collections.Generic;
namespace MyInterfaces
{
    public interface IActionable<T, T2>
    {
        int _Seat { get; set; }
        string _Id { get; set; }
        int _diceRandomValue { get; set; }
        List<token> _myTokens { get; set; }
        PlayerMode _BoardOf
        {
            get;
            set;
        }
        int _lazymoves { get; set; }
        int _opentokens { get; set; }
        int _goaledtokens { get; set; }
        HmUiThings _uiarea { get; set; }
        ColorModes _homeColor { get; set; }
        bool _islazy { get; set; }
        bool _isMovedToken { get; set; }
        bool _isRolledDice { get; set; }
        bool _CanOpenToken { get; set; }
        bool _connected { get; set; }
        void OnTurn();
        void OnDiceRoll(T id);
        
        void OnTurnFinished();
        void OnExtraTurn();
        void OnGoaledToken(T id);
        void OnMoveToken(T id, T2 oldpos);
        void OnOpenToken(T id);
        void OnHit(T id);
        void OnLeave();
        void OnTokenChange(TokenState ts, string id);

    }
    public interface ISelectable
    {
        void OnTokenOpen();
        void OnTokenMove();
    }
}