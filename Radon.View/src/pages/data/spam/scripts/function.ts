import { apiPutNumber } from "@/lib/common/apiMethods.ts";
import type { SpamType } from "@/pages/data/spam/scripts/define";
import { MessagePlugin } from "tdesign-vue-next";

export async function spamAppend(type: SpamType, text: string) {
  const request = {
    type: type,
    text: text.trim(),
  };
  const response = (await apiPutNumber("/api/spam/append", request)).data;

  if (response > 0) {
    void MessagePlugin.success(`追加成功: ${response}`);
    return true;
  } else {
    switch (response) {
      case -1:
        await MessagePlugin.warning("追加失败: 数据库错误");
        break;
      case -2:
        await MessagePlugin.warning("追加失败: 类型为空");
        break;
      case -6:
        await MessagePlugin.warning("追加失败: 内部错误");
        break;
      case -8:
        await MessagePlugin.warning("追加失败: 重复内容");
        break;
      case -10:
        await MessagePlugin.warning("追加失败: 类型错误");
        break;
      default:
        await MessagePlugin.error(`追加失败: ${response}`);
    }
    return false;
  }
}
