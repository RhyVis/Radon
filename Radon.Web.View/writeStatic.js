import fs from "fs-extra";

try {
  fs.removeSync("../Radon.Web/wwwroot");
} catch (e) {
  console.error(e);
}

try {
  fs.ensureDirSync("../Radon.Web/wwwroot", null);
} catch (e) {
  console.error(e);
}

try {
  fs.moveSync("./dist", "../Radon.Web/wwwroot", { overwrite: true });
} catch (e) {
  console.error(e);
}
