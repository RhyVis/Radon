import { authValidate } from "@/lib/common/authMethods";
import i18n from "@/locale";
import { get, useOnline } from "@vueuse/core";
import { MessagePlugin } from "tdesign-vue-next";
import type { RouteLocationNormalizedGeneric } from "vue-router";

const { t } = i18n.global;

export const onlineGuard = async (to: RouteLocationNormalizedGeneric) => {
  if (to.meta.online) {
    const b = get(useOnline());
    if (b) {
      return true;
    } else {
      await MessagePlugin.warning(t("message.pageNeedNetwork"));
      return false;
    }
  } else {
    return true;
  }
};

export const authGuard = async (to: RouteLocationNormalizedGeneric) => {
  if (to.meta.auth) {
    const b = await authValidate();
    if (b) {
      return true;
    } else {
      void MessagePlugin.warning(t("message.authInvalid"));
      return "/error";
    }
  } else {
    return true;
  }
};
