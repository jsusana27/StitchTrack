import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";

//Define a type for the basic material structure
type Material = {
  finishedProductMaterialsID: number;
  finishedProductsID: number;
  materialType: "Yarn" | "SafetyEyes" | "Stuffing";
  materialID: number;
  quantityUsed: number;
};

//Define a type for the detailed material with additional properties
type DetailedMaterial = Material & {
  brand?: string;
  color?: string;
  fiberWeight?: string;
  price?: number;
  sizeInMM?: number; //Include lowercase variations of size
  size?: number; //Include the "size" property explicitly
  PricePerFivelbs?: number;
  pricePerFivelbs?: number;
};

const LookAtDataMaterialsForProductOutput: React.FC = () => {
  const navigate = useNavigate();
  const { productName } = useParams<{ productName: string }>();
  const [materials, setMaterials] = useState<DetailedMaterial[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);

  //Fetch materials on component mount
  useEffect(() => {
    const fetchMaterials = async () => {
      setLoading(true);
      setError(null);

      try {
        //Step 1: Fetch the base materials for the given product name
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/FinishedProductMaterial/get-materials-by-name`,
          { params: { finishedProductName: productName } }
        );

        const baseMaterials: Material[] = response.data || [];

        //Step 2: Fetch detailed information for each material based on its type and ID
        const detailedMaterialsPromises = baseMaterials.map(
          async (material: Material): Promise<DetailedMaterial> => {
            let detailedInfo = null;

            if (material.materialType === "Yarn") {
              detailedInfo = await axios.get(
                `${process.env.REACT_APP_API_URL}/Yarn/${material.materialID}`
              );
            } else if (material.materialType === "SafetyEyes") {
              detailedInfo = await axios.get(
                `${process.env.REACT_APP_API_URL}/SafetyEye/${material.materialID}`
              );
            } else if (material.materialType === "Stuffing") {
              detailedInfo = await axios.get(
                `${process.env.REACT_APP_API_URL}/Stuffing/${material.materialID}`
              );
            }

            //Merge the detailed info into the material object
            return { ...material, ...detailedInfo?.data };
          }
        );

        //Wait for all detailed data to be fetched
        const detailedMaterials = await Promise.all(detailedMaterialsPromises);

        //Step 3: Update the state with detailed materials
        setMaterials(detailedMaterials);
      } catch (err) {
        setError("Failed to fetch materials for this product.");
      } finally {
        setLoading(false);
      }
    };

    if (productName) {
      fetchMaterials();
    }
  }, [productName]);

  //Filter materials into Yarn, SafetyEyes, and Stuffing arrays
  const yarnMaterials = materials.filter(
    (material) => material.materialType === "Yarn"
  );
  const safetyEyesMaterials = materials.filter(
    (material) => material.materialType === "SafetyEyes"
  );
  const stuffingMaterials = materials.filter(
    (material) => material.materialType === "Stuffing"
  );

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Materials needed for {productName}</h1>
        <hr className="title-separator" />
      </header>

      {/* Show loading spinner */}
      {loading && <div>Loading...</div>}

      {/* Show error message */}
      {error && <div className="error">{error}</div>}

      {/* Yarn Table */}
      {!loading && yarnMaterials.length > 0 && (
        <div className="materials-list">
          <h2>Yarn Needed</h2>
          <table className="table">
            <thead>
              <tr>
                <th>Brand/Shape</th>
                <th>Quantity Used</th>
                <th>Color</th>
                <th>Fiber Weight</th>
                <th>Price</th>
              </tr>
            </thead>
            <tbody>
              {yarnMaterials.map((material) => (
                <tr key={material.finishedProductMaterialsID}>
                  <td>{material.brand || "N/A"}</td>
                  <td>{material.quantityUsed}</td>
                  <td>{material.color || "N/A"}</td>
                  <td>{material.fiberWeight || "N/A"}</td>
                  <td>{material.price ? `$${material.price}` : "N/A"}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Safety Eyes Table */}
      {!loading && safetyEyesMaterials.length > 0 && (
        <div className="materials-list">
          <h2>Safety Eyes Needed</h2>
          <table className="table">
            <thead>
              <tr>
                <th>Size In MM</th>
                <th>Quantity Used</th>
                <th>Color</th>
                <th>Price</th>
              </tr>
            </thead>
            <tbody>
              {safetyEyesMaterials.map((material) => (
                <tr key={material.finishedProductMaterialsID}>
                  <td>{material.sizeInMM || material.size || "N/A"}</td>
                  <td>{material.quantityUsed}</td>
                  <td>{material.color || "N/A"}</td>
                  <td>{material.price ? `$${material.price}` : "N/A"}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Stuffing Table */}
      {!loading && stuffingMaterials.length > 0 && (
        <div className="materials-list">
          <h2>Stuffing Needed</h2>
          <table className="table">
            <thead>
              <tr>
                <th>Brand/Type</th>
                <th>Quantity Used</th>
                <th>Price Per 5 lbs</th>
              </tr>
            </thead>
            <tbody>
              {stuffingMaterials.map((material) => (
                <tr key={material.finishedProductMaterialsID}>
                  <td>{material.brand || "N/A"}</td>
                  <td>{material.quantityUsed}</td>
                  <td>
                    {material.PricePerFivelbs || material.pricePerFivelbs
                      ? `$${
                          material.PricePerFivelbs || material.pricePerFivelbs
                        }`
                      : "N/A"}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Back button */}
      <div className="button-group">
        <button onClick={() => navigate("/look-at-data")} className="button">
          Back
        </button>
      </div>
    </div>
  );
};

export default LookAtDataMaterialsForProductOutput;
