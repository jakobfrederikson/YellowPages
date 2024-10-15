using YellowPages;
using YellowPages.Menu;

using var db = new YellowPagesContext();
var menu = new StartMenu(db);

menu.Start();
