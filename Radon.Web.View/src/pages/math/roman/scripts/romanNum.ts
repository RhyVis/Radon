const romanMap: [number, string][] = [
  [1000000, "M̅"],
  [900000, "C̅M̅"],
  [500000, "D̅"],
  [400000, "C̅D̅"],
  [100000, "C̅"],
  [90000, "X̅C̅"],
  [50000, "L̅"],
  [40000, "X̅L̅"],
  [10000, "X̅"],
  [9000, "MX̅"],
  [5000, "V̅"],
  [4000, "MV̅"],
  [1000, "M"],
  [900, "CM"],
  [500, "D"],
  [400, "CD"],
  [100, "C"],
  [90, "XC"],
  [50, "L"],
  [40, "XL"],
  [10, "X"],
  [9, "IX"],
  [5, "V"],
  [4, "IV"],
  [1, "I"],
];

function intToRoman(num: number): string {
  let result = "";
  for (const [value, symbol] of romanMap) {
    while (num >= value) {
      result += symbol;
      num -= value;
    }
  }
  return result;
}

export { intToRoman };
