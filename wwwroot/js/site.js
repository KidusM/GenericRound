
    //const CategoryList = ["Services", "Utility", "Doctors", "Insurance", "Airfare", "Tuition", "Canteen", "Other Services"];
    //fruits.includes("Mango") 


function display_Fields(Category)
{
    var CatName = Category;
       //CatnName = document.getElementById(Category).Text;
    //alert(CatName);


    var idName = document.getElementsByClassName("HideAll");

    for (var i = 0; i < idName.length; i++) {
        
        idName[i].style.display = 'none';
    }

    var elements = document.getElementsByClassName(CatName);

    for (var i = 0; i < elements.length; i++)
    {
        elements[i].style.display = 'block';
    }
    //alert("Elements " + elements);

    //document.getElementByClassName('HideAll').display = 'none';
    //document.getElementByClassName("'" + CatName + "'").display = 'block';
  
}