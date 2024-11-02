import fs from "fs-extra";

try {
  fs.removeSync("../Radon.App/wwwroot");
} catch (e) {
  console.error(e);
}

try {
  fs.ensureDirSync("../Radon.App/wwwroot", null);
} catch (e) {
  console.error(e);
}

try {
  fs.moveSync("./dist", "../Radon.App/wwwroot", { overwrite: true });
} catch (e) {
  console.error(e);
}
