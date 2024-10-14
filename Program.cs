using YellowPages;
using YellowPages.Menu;

using var db = new YellowPagesContext();
var menu = new Menu(db);

menu.StartMenu();
