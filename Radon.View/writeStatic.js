import fs from "fs-extra";

try {
  fs.removeSync("../Radon.Arc/wwwroot");
} catch (e) {
  console.error(e);
}

try {
  fs.ensureDirSync("../Radon.Arc/wwwroot", null);
} catch (e) {
  console.error(e);
}

try {
  fs.moveSync("./dist", "../Radon.Arc/wwwroot", { overwrite: true });
} catch (e) {
  console.error(e);
}
