using Grpc.Core;
using Grpc.Net.Client;
using System;

namespace GrpcShopServiceClient
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //CLIENT For ProductService
            var clientProdS = new ProductCRUD.ProductCRUDClient(channel);

            //Adding new product
            var prodName = Console.ReadLine();
            var prodDescr = Console.ReadLine();
            double prodPrice = Convert.ToDouble(Console.ReadLine());
            var prodCatId = Convert.ToInt32(Console.ReadLine());

            var pRep = await clientProdS.AddProductAsync(new ProductCreate { Name = prodName, Description = prodDescr, Price = prodPrice, CategoryId = prodCatId });
            Console.WriteLine($"New product details:{pRep.Id}_{ pRep.Name}_{pRep.Description}_{pRep.Price}_\n--->{pRep.Category.Id}__{pRep.Category.Name}");

            //Retrieve product by id
            Console.WriteLine("You can enter id of product to get detailed info");
            var pId = Convert.ToInt32(Console.ReadLine());
            var singleP = await clientProdS.GetProductByIdAsync(new ProductLookup { Id = pId });
            Console.WriteLine($"Requested product details:{singleP.Id}_{ singleP.Name}_{singleP.Description}_{singleP.Price}_\n--->{singleP.Category.Id}__{singleP.Category.Name}");

            Console.WriteLine("You can enter id of exact category to get detailed info");
            var catID = Convert.ToInt32(Console.ReadLine());
            using (var call = clientProdS.GetProductsByCategoryId(new CategoryLookup { Id = catID }))
            {
                while(await call.ResponseStream.MoveNext())
                {
                    var p = call.ResponseStream.Current;
                    Console.WriteLine($"Requested product details:{p.Id}_{ p.Name}_{p.Description}_{p.Price}_\n--->{p.Category.Id}__{p.Category.Name}");
                }
            }

            //User for updating the category id of existing product

            Console.WriteLine("Please, enter the id of the product and new category id");
            var pID = Convert.ToInt32(Console.ReadLine());
            var newCatId = Convert.ToInt32(Console.ReadLine());

            var updProd = await clientProdS.UpdateProductCategoryAsync(new ProductUpdate { Id = pID, CategoryId = newCatId });
            Console.WriteLine($"Updated category product details:{updProd.Id}_{ updProd.Name}_{updProd.Description}_{updProd.Price}_\n--->{updProd.Category.Id}__{updProd.Category.Name}");

            //CLIENT For CategoryService

            /*var clientCategoryServ = new CategoryCRUD.CategoryCRUDClient(channel);
            
            //Used for AddCategory method
            var input = Console.ReadLine();
            var reply = await clientCategoryServ.AddCategoryAsync(
                              new CategoryCreate { Name = input });
            Console.WriteLine("New category info: " + reply.Id+" "+reply.Name);
            
            //Used for GetAllCategories method
            Console.WriteLine("Here is the list of all categories:");
            using(var call = clientCategoryServ.GetAllCategories(new AllLookup { }))
            {
                while(await call.ResponseStream.MoveNext())
                {
                    var rep = call.ResponseStream.Current;
                    Console.WriteLine("Category-> " + rep.Id + "-" + rep.Name);
                }
            }

            //Used for GetCategoryById method
            Console.WriteLine("Now you can type the id of exact category:");
            var idCat = Convert.ToInt32(Console.ReadLine());
            var repCat = await clientCategoryServ.GetCategoryByIdAsync(new CategoryLookup { Id = idCat });
            Console.WriteLine("Requested category info: " + repCat.Id + " " + repCat.Name);

            //Used for DeleteCategory method
            Console.WriteLine($"Requested category with id={idCat} is going to be deleted");
            var delCat = await clientCategoryServ.DeleteCategoryAsync(new CategoryLookup { Id=idCat});
            
            //Calling to retrieve all categories
            Console.WriteLine("Here is the list of all categories:");
            using (var call = clientCategoryServ.GetAllCategories(new AllLookup { }))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var rep = call.ResponseStream.Current;
                    Console.WriteLine("Category-> " + rep.Id + "-" + rep.Name);
                }
            }

            //Used for UpdateCategory method
            Console.WriteLine("Please, now enter the id of the existing category and its new name");
            var idCatUpd = Convert.ToInt32(Console.ReadLine());
            var newName = Console.ReadLine();
            var updCatRep = await clientCategoryServ.UpdateCategoryAsync(new CategoryInfo { Id = idCatUpd, Name = newName });
            Console.WriteLine("Updated category info: " + updCatRep.Id + " " + updCatRep.Name);*/
        }
    }
}
