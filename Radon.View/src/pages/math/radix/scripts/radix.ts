const numberSet = "0123456789";
const alphabetCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "abcdefghijklmnopqrstuvwxyz";
const greekCharset = "αβγδεζηθικλμνξοπρστυφχψω" + "ΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩ";
const russianCharset = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" + "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
const latinExtendedCharset =
  "ĀĂĄĆĈĊČĎĐĒĔĖĘĚĜĞĠĢĤĦĨĪĬĮİĲĴĶĹĻĽĿŁŃŅŇŊŌŎŐŒŔŖŘŚŜŞŠŢŤŦŨŪŬŮŰŲŴŶŸŹŻŽāăąćĉċčďđēĕėęěĝğġģĥħĩīĭįıĳĵķĸĺļľŀłńņňŋōŏőœŕŗřśŝşšţťŧũūŭůűųŵŷÿźżž";
const latinSupplementCharset = "ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþ";

const mixedLatinCharset = latinExtendedCharset + latinSupplementCharset;

export const basicCharset = numberSet + alphabetCharset;
export const extendedCharset = numberSet + alphabetCharset + greekCharset + russianCharset + mixedLatinCharset;

export const radixVal = (
  input: string,
  iRadix: number,
  oRadix: number,
  iCharset: string = basicCharset,
  oCharset: string = basicCharset,
) => {
  try {
    if (iCharset.length < 2 || oCharset.length < 2) return "基数过小";
    if (iRadix > iCharset.length || oRadix > oCharset.length) return "字符集过短";

    let decimalVal: bigint = BigInt(0);
    for (let i = 0; i < input.length; i++) {
      decimalVal = decimalVal * BigInt(iRadix) + BigInt(iCharset.indexOf(input[i]));
    }

    let result = "";
    if (decimalVal === BigInt(0)) {
      return oCharset[0];
    }

    while (decimalVal > 0) {
      const remainder = decimalVal % BigInt(oRadix);
      result = oCharset[Number(remainder)] + result;
      decimalVal = decimalVal / BigInt(oRadix);
    }

    return result;
  } catch (e) {
    console.error(e);
    return "转换失败";
  }
};

export const radixValExtended = (
  input: number,
  radix: number = extendedCharset.length,
  iCharset: string = numberSet,
  oCharset: string = extendedCharset,
) => radixVal(input.toString(), 10, radix, iCharset, oCharset);
