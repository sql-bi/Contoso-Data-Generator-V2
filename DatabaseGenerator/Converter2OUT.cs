using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseGenerator
{

    public static class Converter2OUT
    {

        public static void EnrichProducts(List<Product> inProductList, List<Category> inCategoryList, List<SubCategory> inSubcategoryList)
        {
            //var outProductList = new List<ModelsOUT.ProductOUT>();

            foreach (var inProduct in inProductList)
            {
                SubCategory subCat = inSubcategoryList.Single(x => x.SubCategoryID == inProduct.SubCategoryID);
                Category cat = inCategoryList.Single(x => x.CategoryID == subCat.CategoryID);

                //var outProduct = new ModelsOUT.ProductOUT()
                //{
                //    ProductKey = inProduct.ProductID,
                //    ProductCode = inProduct.ProductCode,
                //    ProductName = inProduct.ProductName,
                //    Manufacturer = inProduct.Manufacturer,
                //    Brand = inProduct.Brand,
                //    Color = inProduct.Color,
                //    WeightUnit = inProduct.WeightUnit,
                //    Weight = inProduct.Weight,
                //    Cost = inProduct.Cost,
                //    Price = inProduct.Price,
                //    SubCategoryCode = inProduct.SubCategoryID,
                //    SubCategory = subCat.SubCategoryName,
                //    CategoryCode = cat.CategoryID,
                //    Category = cat.CategoryName
                //};

                inProduct.CategoryID = cat.CategoryID;
                inProduct.CategoryName = cat.CategoryName;
                inProduct.SubCategoryName = subCat.SubCategoryName;


                //outProductList.Add(outProduct);
            }

            //return outProductList;
        }


        public static void EnrichStores(List<Store> storesIN, List<GeoArea> geoAreas)
        {
            //var items = new List<ModelsOUT.StoreOUT>();

            foreach (var storeIN in storesIN)
            {
                var geoArea = geoAreas.Single(x => x.GeoAreaID == storeIN.GeoAreaID);

                //items.Add(
                //    new ModelsOUT.StoreOUT()
                //    {
                //        StoreKey = storeIN.StoreID,
                //        StoreCode = storeIN.StoreCode,
                //        Country = geoArea.Country,
                //        State = geoArea.StateLongName,
                //        Name = storeIN.Description,
                //        SquareMeters = storeIN.SquareMeters,
                //        OpenDate = storeIN.OpenDate,
                //        CloseDate = storeIN.CloseDate,
                //        Status = storeIN.Status
                //    });

                storeIN.Country = geoArea.Country;
                storeIN.State = geoArea.StateLongName;
            }

            //return items;
        }

    }

}
