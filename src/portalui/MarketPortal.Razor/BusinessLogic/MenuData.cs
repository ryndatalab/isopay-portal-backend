namespace MarketPortal.Razor.BusinessLogic
{
    public class MenuData
    {
        public string Head { get; set; }
        public string Menu { get; set; }
        public string Link { get; set; }

        public static List<MenuData> GetMenuDatas()
        {
            List<MenuData> menuDatas = new List<MenuData>()
            {
                new MenuData(){Head= "Marketplace",Menu="App",Link="app"} ,
                new MenuData(){Head= "Marketplace",Menu="Service",Link="Service"} ,
            new MenuData(){Head= "Marketplace",Menu="Marketplace Managmement",Link="MarketplaceManagement"} ,

            new MenuData(){Head= "App Management",Menu="Manage App",Link="ManageApp"} ,
             new MenuData(){Head= "App Management",Menu="App subscription",Link="AppSubscription"} ,
             new MenuData(){Head= "Service Management",Menu="Service App",Link="ManageService"} ,
             new MenuData(){Head= "Service Management",Menu="Service subscription",Link="ServiceSubscription"} ,

           new MenuData(){Head= "User management",Menu="App user",Link="AppUser"} ,
           new MenuData(){Head= "User management",Menu="Portal user",Link="PortalUser"} ,

            new MenuData(){Head= "Partner Network",Menu="Global Parntners",Link="GlobalPartner"} ,
            new MenuData(){Head= "Partner Network",Menu="Invite",Link="Invite"} ,
            new MenuData(){Head= "Partner Network",Menu="Partner approval",Link="InviteApproval"} ,

            new MenuData(){Head= "Tech Integration",Menu="EDC",Link="EDC"} ,
            new MenuData(){Head= "Tech Integration",Menu="Tech user",Link="TechUser"} ,
            new MenuData(){Head= "Tech Integration",Menu="IDP",Link="IDPConfiguration"} ,

            new MenuData(){Head= "Data hub",Menu="Digital Twin",Link="DigitalTwin"} ,
            new MenuData(){Head= "Data hub",Menu="Semantic Twin",Link="Semantichub"} ,


            new MenuData(){Head= "Use case",Menu="Participation",Link="UsecaseParticipation"} ,
            new MenuData(){Head= "Use case",Menu="Management",Link="UsecaseManagement"} ,

            new MenuData(){Head= "Profile",Menu="Account",Link="Account"} ,
            new MenuData(){Head= "Profile",Menu="My Organization",Link="MyCompany"} ,
            new MenuData(){Head= "Profile",Menu="Notication",Link="Notification"} ,
            new MenuData(){Head= "Profile",Menu="Wallet",Link="Wallet"} ,
            new MenuData(){Head= "Profile",Menu="Sign-Out",Link="SignOut"} ,
            }
                ;
            return menuDatas;
        }
    }
}
