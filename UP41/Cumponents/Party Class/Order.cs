using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UP41.Cumponents
{
    public partial class Order
    {
        public StatusOrder CurrentStatus
        {
            get { return StatusOrder.First(y => y.Id == StatusOrder.Max(x => x.Id)); }
        }

        public DateTime GetOrderDate
        {
            get
            {
                if (DateOrder == new DateTime())
                    return DateTime.Now.Date;
                return DateOrder.Date;
            }
        }

        public Visibility CanDelete
        {
            get
            {
                int RoleId = (int)App.db.User.Where(x => x.Login == App.currentUser).First().RoleId;
                if (CurrentStatus.IdStatus == 1 && (RoleId == 4 || RoleId == 5))
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }
        }

        public Visibility CanEdit
        {
            get
            {
                if (LoginManager == App.currentUser) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }
        public Dictionary<Material, decimal> GetMaterials()
        {
            var materialList = new Dictionary<Material, decimal>();
            Dictionary<Product, int> products = new Dictionary<Product, int>();
            if (Product != null) products = Product.GetProductDetails();

            foreach (var pro in products)
            {
                foreach (var mat in pro.Key.ProductMaterial)
                {
                    if (!materialList.Any(x => x.Key.Article == mat.MaterialArticle))
                        materialList.Add(mat.Material, mat.Count);
                    else
                        materialList[mat.Material] += mat.Count;
                }
            }
            return materialList;
        }

        public Dictionary<Accessories, decimal> GetAccessories()
        {
            var accessoriesList = new Dictionary<Accessories, decimal>();
            Dictionary<Product, int> products = new Dictionary<Product, int>();
            if (Product != null) products = Product.GetProductDetails();

            foreach (var pro in products)
            {
                foreach (var mat in pro.Key.ProductAccessories)
                {
                    if (!accessoriesList.Any(x => x.Key.Article == mat.AccessoriesArticle))
                        accessoriesList.Add(mat.Accessories, mat.Count);
                    else
                        accessoriesList[mat.Accessories] += mat.Count;
                }
            }
            return accessoriesList;
        }
    }
}
