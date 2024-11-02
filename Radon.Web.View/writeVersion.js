import fs from "fs";

// Build time record

const date = new Date();
const version = date.getTime();
const versionObject = { compileTime: version };
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
