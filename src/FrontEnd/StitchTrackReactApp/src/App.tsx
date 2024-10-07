import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";

import Home from "./components/Home"; //Home component

//=========================== LOOK AT DATA COMPONENTS SECTION ===========================

//Main Look At Data Page
import LookAtData from "./components/LookAtData/LookAtData";

//Yarn Components for Look At Data
import LookAtDataYarn from "./components/LookAtData/LookAtDataYarn/LookAtDataYarn";
import LookAtDataYarnBrands from "./components/LookAtData/LookAtDataYarn/LookAtDataYarnBrands";
import LookAtDataYarnColors from "./components/LookAtData/LookAtDataYarn/LookAtDataYarnColors";
import LookAtDataYarnFiberTypes from "./components/LookAtData/LookAtDataYarn/LookAtDataYarnFiberTypes";
import LookAtDataYarnFiberWeights from "./components/LookAtData/LookAtDataYarn/LookAtDataYarnFiberWeights";
import LookAtDataYarnPrices from "./components/LookAtData/LookAtDataYarn/LookAtDataYarnPrices";
import LookAtDataYarnYardagePerSkein from "./components/LookAtData/LookAtDataYarn/LookAtDataYarnYardagePerSkein";
import LookAtDataYarnGramsPerSkein from "./components/LookAtData/LookAtDataYarn/LookAtDataYarnGramsPerSkein";
import LookAtDataYarnNumberInStock from "./components/LookAtData/LookAtDataYarn/LookAtDataYarnNumberInStock";

//Safety Eyes Components for Look At Data
import LookAtDataSafetyEyes from "./components/LookAtData/LookAtDataSafetyEyes/LookAtDataSafetyEyes";
import LookAtDataSafetyEyesSizes from "./components/LookAtData/LookAtDataSafetyEyes/LookAtDataSafetyEyesSizes";
import LookAtDataSafetyEyesColors from "./components/LookAtData/LookAtDataSafetyEyes/LookAtDataSafetyEyesColors";
import LookAtDataSafetyEyesShapes from "./components/LookAtData/LookAtDataSafetyEyes/LookAtDataSafetyEyesShapes";
import LookAtDataSafetyEyesPrices from "./components/LookAtData/LookAtDataSafetyEyes/LookAtDataSafetyEyesPrices";
import LookAtDataSafetyEyesStock from "./components/LookAtData/LookAtDataSafetyEyes/LookAtDataSafetyEyesStock";

//Stuffing Components for Look At Data
import LookAtDataStuffing from "./components/LookAtData/LookAtDataStuffing/LookAtDataStuffing";
import LookAtDataStuffingBrands from "./components/LookAtData/LookAtDataStuffing/LookAtDataStuffingBrands";
import LookAtDataStuffingTypes from "./components/LookAtData/LookAtDataStuffing/LookAtDataStuffingTypes";
import LookAtDataStuffingPrices from "./components/LookAtData/LookAtDataStuffing/LookAtDataStuffingPrices";

//Finished Products Components for Look At Data
import LookAtDataProducts from "./components/LookAtData/LookAtDataProducts/LookAtDataProducts";
import LookAtDataProductsCostToMake from "./components/LookAtData/LookAtDataProducts/LookAtDataProductsCostToMake";
import LookAtDataProductsSalePrice from "./components/LookAtData/LookAtDataProducts/LookAtDataProductsSalePrice";
import LookAtDataProductsStock from "./components/LookAtData/LookAtDataProducts/LookAtDataProductsStock";
import LookAtDataProductsNames from "./components/LookAtData/LookAtDataProducts/LookAtDataProductsNames";
import LookAtDataProductsTime from "./components/LookAtData/LookAtDataProducts/LookAtDataProductsTime";

//Materials Needed to Make a Product
import LookAtDataMaterialsForProductInput from "./components/LookAtData/LookAtDataMaterialsForProduct/LookAtDataMaterialsForProductInput";
import LookAtDataMaterialsForProductOutput from "./components/LookAtData/LookAtDataMaterialsForProduct/LookAtDataMaterialsForProductOutput";

//Customers & Orders
import LookAtDataAllCustomers from "./components/LookAtData/LookAtDataAllCustomers";
import LookAtDataAllOrders from "./components/LookAtData/LookAtDataAllOrders";

//Customer Purchase History
import LookAtDataCustomerAllPurchasedInput from "./components/LookAtData/LookAtDataCustomerAllPurchased/LookAtDataCustomerAllPurchasedInput";
import LookAtDataCustomerAllPurchasedOutput from "./components/LookAtData/LookAtDataCustomerAllPurchased/LookAtDataCustomerAllPurchasedOutput";

//Sales Stats
import LookAtDataSaleStatsInput from "./components/LookAtData/LookAtDataSaleStats/LookAtDataSaleStatsInput";
import LookAtDataSaleStatsOutput from "./components/LookAtData/LookAtDataSaleStats/LookAtDataSaleStatsOutput";

//=========================== ADD DATA COMPONENTS SECTION ===========================
import AddData from "./components/AddData/AddData";
import AddDataYarn from "./components/AddData/AddDataYarn";
import AddDataSafetyEyes from "./components/AddData/AddDataSafetyEyes";
import AddDataStuffing from "./components/AddData/AddDataStuffing";
import AddDataProduct from "./components/AddData/AddDataProduct";

//Adding Materials to a Product
import AddProductMaterialName from "./components/AddData/AddDataProductMaterial/AddDataProductMaterialName";
import AddDataProductMaterialType from "./components/AddData/AddDataProductMaterial/AddDataProductMaterialType";
import AddDataProductMaterialYarn from "./components/AddData/AddDataProductMaterial/AddDataProductMaterialYarn";
import AddProductMaterialSafetyEyes from "./components/AddData/AddDataProductMaterial/AddProductMaterialSafetyEyes";
import AddProductMaterialStuffing from "./components/AddData/AddDataProductMaterial/AddDataProductMaterialStuffing";

import AddDataCustomer from "./components/AddData/AddDataCustomer";
import AddDataOrder from "./components/AddData/AddDataOrder";

//=========================== MODIFY DATA COMPONENTS SECTION ===========================
import ModifyData from "./components/ModifyData/ModifyData";

//Modify Quantities
import ModifyDataQuantity from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantity";
import ModifyDataQuantityProductCheck from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantityProduct/ModifyDataQuantityProductCheck";
import ModifyDataQuantityProductUpdate from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantityProduct/ModifyDataQuantityProductUpdate";
import ModifyDataQuantityYarnCheck from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantityYarn/ModifyDataQuantityYarnCheck";
import ModifyDataQuantityYarnUpdate from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantityYarn/ModifyDataQuantityYarnUpdate";
import ModifyDataQuantitySafetyEyesCheck from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantitySafetyEyes/ModifyDataQuantitySafetyEyesCheck";
import ModifyDataQuantitySafetyEyesUpdate from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantitySafetyEyes/ModifyDataQuantitySafetyEyesUpdate";

//Modify Materials in a Product
import ModifyDataQuantityProductMaterialName from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantityProductMaterial/ModifyDataQuantityProductMaterialName";
import ModifyDataQuantityProductMaterialType from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantityProductMaterial/ModifyDataQuantityProductMaterialType";
import ModifyDataQuantityProductMaterialYarn from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantityProductMaterial/ModifyDataQuantityProductMaterialYarn";
import ModifyDataQuantityProductMaterialSafetyEyes from "./components/ModifyData/ModifyDataQuantity/ModifyDataQuantityProductMaterial/ModifyDataQuantityProductMaterialSafetyEyes";

//Modify Finished Product Details
import ModifyDataProductPrice from "./components/ModifyData/ModifyDataProductPrice";
import ModifyDataProductTime from "./components/ModifyData/ModifyDataProductTime";
import ModifyDataCustomer from "./components/ModifyData/ModifyDataCustomer";

//=========================== DELETE DATA COMPONENTS SECTION ===========================
import DeleteData from "./components/DeleteData/DeleteData";
import DeleteDataYarn from "./components/DeleteData/DeleteDataYarn";
import DeleteDataSafetyEyes from "./components/DeleteData/DeleteDataSafetyEyes";
import DeleteDataStuffing from "./components/DeleteData/DeleteDataStuffing";
import DeleteDataProduct from "./components/DeleteData/DeleteDataProduct";

//Deleting Materials from a Product
import DeleteDataProductMaterialName from "./components/DeleteData/DeleteDataProductMaterial/DeleteDataProductMaterialName";
import DeleteDataProductMaterialType from "./components/DeleteData/DeleteDataProductMaterial/DeleteDataProductMaterialType";
import DeleteDataProductMaterialYarn from "./components/DeleteData/DeleteDataProductMaterial/DeleteDataProductMaterialYarn";
import DeleteDataProductMaterialSafetyEyes from "./components/DeleteData/DeleteDataProductMaterial/DeleteDataProductMaterialSafetyEyes";
import DeleteDataProductMaterialStuffing from "./components/DeleteData/DeleteDataProductMaterial/DeleteDataProductMaterialStuffing";

//Deleting a Customer
import DeleteDataCustomer from "./components/DeleteData/DeleteDataCustomer";
import DeleteDataOrder from "./components/DeleteData/DeleteDataOrder";

// =========================== MAIN APP COMPONENT ===========================
const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        {/* Home Route */}
        <Route path="/" element={<Home />} />

        {/* LOOK AT DATA ROUTES */}
        <Route path="/look-at-data" element={<LookAtData />} />

        {/* Yarn Routes */}
        <Route path="/look-at-data/yarn" element={<LookAtDataYarn />} />
        <Route
          path="/look-at-data/yarn/brands"
          element={<LookAtDataYarnBrands />}
        />
        <Route
          path="/look-at-data/yarn/colors"
          element={<LookAtDataYarnColors />}
        />
        <Route
          path="/look-at-data/yarn/fiber-types"
          element={<LookAtDataYarnFiberTypes />}
        />
        <Route
          path="/look-at-data/yarn/fiber-weights"
          element={<LookAtDataYarnFiberWeights />}
        />
        <Route
          path="/look-at-data/yarn/prices"
          element={<LookAtDataYarnPrices />}
        />
        <Route
          path="/look-at-data/yarn/yardage"
          element={<LookAtDataYarnYardagePerSkein />}
        />
        <Route
          path="/look-at-data/yarn/grams"
          element={<LookAtDataYarnGramsPerSkein />}
        />
        <Route
          path="/look-at-data/yarn/number-in-stock"
          element={<LookAtDataYarnNumberInStock />}
        />

        {/* Safety Eyes Routes */}
        <Route
          path="/look-at-data/safety-eyes"
          element={<LookAtDataSafetyEyes />}
        />
        <Route
          path="/look-at-data/safety-eyes/sizes"
          element={<LookAtDataSafetyEyesSizes />}
        />
        <Route
          path="/look-at-data/safety-eyes/colors"
          element={<LookAtDataSafetyEyesColors />}
        />
        <Route
          path="/look-at-data/safety-eyes/shapes"
          element={<LookAtDataSafetyEyesShapes />}
        />
        <Route
          path="/look-at-data/safety-eyes/prices"
          element={<LookAtDataSafetyEyesPrices />}
        />
        <Route
          path="/look-at-data/safety-eyes/stock"
          element={<LookAtDataSafetyEyesStock />}
        />

        {/* Stuffing Routes */}
        <Route path="/look-at-data/stuffing" element={<LookAtDataStuffing />} />
        <Route
          path="/look-at-data/stuffing/brands"
          element={<LookAtDataStuffingBrands />}
        />
        <Route
          path="/look-at-data/stuffing/types"
          element={<LookAtDataStuffingTypes />}
        />
        <Route
          path="/look-at-data/stuffing/prices"
          element={<LookAtDataStuffingPrices />}
        />

        {/* Finished Products Routes */}

        <Route
          path="/look-at-data/finished-products"
          element={<LookAtDataProducts />}
        />
        <Route
          path="/look-at-data/finished-products/names"
          element={<LookAtDataProductsNames />}
        />
        <Route
          path="/look-at-data/finished-products/sorted-by-time"
          element={<LookAtDataProductsTime />}
        />
        <Route
          path="/look-at-data/finished-products/sorted-by-cost"
          element={<LookAtDataProductsCostToMake />}
        />
        <Route
          path="/look-at-data/finished-products/sorted-by-price"
          element={<LookAtDataProductsSalePrice />}
        />
        <Route
          path="/look-at-data/finished-products/sorted-by-stock"
          element={<LookAtDataProductsStock />}
        />

        {/* Materials Needed to Make a Product Routes */}
        <Route
          path="/look-at-data/materials-needed-input"
          element={<LookAtDataMaterialsForProductInput />}
        />
        <Route
          path="/look-at-data/materials-needed-output/:productName"
          element={<LookAtDataMaterialsForProductOutput />}
        />

        {/* All Customers Route */}
        <Route
          path="/look-at-data/customers"
          element={<LookAtDataAllCustomers />}
        />

        {/* All Orders Route */}
        <Route path="/look-at-data/orders" element={<LookAtDataAllOrders />} />

        {/* Customer All Purchased Products Routes */}
        <Route
          path="/look-at-data/customer-purchased-input"
          element={<LookAtDataCustomerAllPurchasedInput />}
        />
        <Route
          path="/look-at-data/customer-purchased/:customerId"
          element={<LookAtDataCustomerAllPurchasedOutput />}
        />

        {/* Sale Stats Routes */}
        <Route
          path="/look-at-data/sale-stats-input"
          element={<LookAtDataSaleStatsInput />}
        />
        <Route
          path="/look-at-data/sale-stats-output/:productId"
          element={<LookAtDataSaleStatsOutput />}
        />

        {/* ADD DATA ROUTES */}
        <Route path="/add-data" element={<AddData />} />
        <Route path="/add-data/yarn" element={<AddDataYarn />} />
        <Route path="/add-data/safety-eyes" element={<AddDataSafetyEyes />} />
        <Route path="/add-data/stuffing" element={<AddDataStuffing />} />
        <Route path="/add-data/product" element={<AddDataProduct />} />
        <Route
          path="/add-data/product-material"
          element={<AddProductMaterialName />}
        />
        <Route
          path="/add-data/product-material/type"
          element={<AddDataProductMaterialType />}
        />
        <Route
          path="/add-data/product-material/type/yarn"
          element={<AddDataProductMaterialYarn />}
        />
        <Route
          path="/add-data/product-material/type/safety-eyes"
          element={<AddProductMaterialSafetyEyes />}
        />
        <Route
          path="/add-data/product-material/type/stuffing"
          element={<AddProductMaterialStuffing />}
        />
        <Route path="/add-data/customer" element={<AddDataCustomer />} />
        <Route path="/add-data/order" element={<AddDataOrder />} />

        {/* MODIFY DATA ROUTES */}
        <Route path="/modify-data" element={<ModifyData />} />
        <Route
          path="/modify-data/quantity-update"
          element={<ModifyDataQuantity />}
        />
        <Route
          path="/modify-data/quantity-update/product-check"
          element={<ModifyDataQuantityProductCheck />}
        />
        <Route
          path="/modify-data/quantity-update/product-check/product-update"
          element={<ModifyDataQuantityProductUpdate />}
        />
        <Route
          path="/modify-data/quantity-update/yarn-check"
          element={<ModifyDataQuantityYarnCheck />}
        />
        <Route
          path="/modify-data/quantity-update/yarn-check/yarn-update"
          element={<ModifyDataQuantityYarnUpdate />}
        />
        <Route
          path="/modify-data/quantity-update/safety-eyes-check"
          element={<ModifyDataQuantitySafetyEyesCheck />}
        />
        <Route
          path="/modify-data/quantity-update/safety-eyes-check/safety-eyes-update"
          element={<ModifyDataQuantitySafetyEyesUpdate />}
        />
        <Route
          path="/modify-data/quantity-update/product-material-name/:productName/product-material-type"
          element={<ModifyDataQuantityProductMaterialType />}
        />
        <Route
          path="/modify-data/quantity-update/product-material-name/:productName/product-material-type/product-material-update/Yarn"
          element={<ModifyDataQuantityProductMaterialYarn />}
        />
        <Route
          path="/modify-data/quantity-update/product-material-name/:productName/product-material-type/product-material-update/SafetyEyes"
          element={<ModifyDataQuantityProductMaterialSafetyEyes />}
        />
        <Route
          path="/modify-data/quantity-update/product-material-name"
          element={<ModifyDataQuantityProductMaterialName />}
        />
        <Route
          path="/modify-data/product-sale-price"
          element={<ModifyDataProductPrice />}
        />
        <Route
          path="/modify-data/product-time"
          element={<ModifyDataProductTime />}
        />
        <Route path="/modify-data/customer" element={<ModifyDataCustomer />} />

        {/* DELETE DATA ROUTES */}
        <Route path="/delete-data" element={<DeleteData />} />
        <Route path="/delete-data/yarn" element={<DeleteDataYarn />} />
        <Route
          path="/delete-data/safety-eyes"
          element={<DeleteDataSafetyEyes />}
        />
        <Route path="/delete-data/stuffing" element={<DeleteDataStuffing />} />
        <Route path="/delete-data/product" element={<DeleteDataProduct />} />
        <Route
          path="/delete-data/product-material"
          element={<DeleteDataProductMaterialName />}
        />
        <Route
          path="/delete-data/product-material/:productName/type"
          element={<DeleteDataProductMaterialType />}
        />
        <Route
          path="/delete-data/product-material/:productName/Yarn"
          element={<DeleteDataProductMaterialYarn />}
        />
        <Route
          path="/delete-data/product-material/:productName/SafetyEyes"
          element={<DeleteDataProductMaterialSafetyEyes />}
        />
        <Route
          path="/delete-data/product-material/:productName/Stuffing"
          element={<DeleteDataProductMaterialStuffing />}
        />
        <Route path="/delete-data/customer" element={<DeleteDataCustomer />} />
        <Route path="/delete-data/order" element={<DeleteDataOrder />} />
      </Routes>
    </Router>
  );
};

export default App;
