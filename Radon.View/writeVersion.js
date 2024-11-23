import fs from "fs";

// Get Version from package.json

const packageJSON = fs.readFileSync("./package.json", "utf8");
const packageObject = JSON.parse(packageJSON);
const packageVersion = packageObject.version;

console.log(`Version found in package.json: ${packageVersion}`);

// Build time record

const date = new Date();
const version = date.getTime();
const versionObject = { clientVersion: packageVersion, compileTime: version };
const versionJSON = JSON.stringify(versionObject);

console.log(`Version confirmed on ${date}`);

// Public folder for server use

fs.writeFile("./public/version.json", versionJSON, err => {
  if (err) console.error(err);
  console.log("Version file written to public");
});

// Assets folder for client use

if (!fs.existsSync("./src/assets/local")) {
  fs.mkdirSync("./src/assets/local", { recursive: true });
}

fs.writeFile("./src/assets/local/version.json", versionJSON, err => {
  if (err) console.error(err);
  console.log("Version file written to src");
});
