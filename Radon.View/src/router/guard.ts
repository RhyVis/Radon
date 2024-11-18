import { authValidate } from "@/lib/common/authMethods";
import i18n from "@/locale";
import { get, useOnline } from "@vueuse/core";
import { MessagePlugin } from "tdesign-vue-next";
import type { NavigationGuardNext, RouteLocationNormalizedGeneric, RouteLocationNormalizedLoaded } from "vue-router";

const { t } = i18n.global;

export const authGuard = async (
  to: RouteLocationNormalizedGeneric,
  _: RouteLocationNormalizedLoaded,
  next: NavigationGuardNext,
) => {
  if (to.meta.auth) {
    const b = await authValidate();
    if (b) {
      next();
    } else {
      await MessagePlugin.warning(t("message.authInvalid"));
      next("/error");
    }
  } else {
    next();
  }
};

export const onlineGuard = async (
  to: RouteLocationNormalizedGeneric,
  _: RouteLocationNormalizedLoaded,
  next: NavigationGuardNext,
) => {
  if (to.meta.online) {
    const b = get(useOnline());
    if (b) {
      next();
    } else {
      await MessagePlugin.warning(t("message.pageNeedNetwork"));
      next("/");
    }
  } else {
    next();
  }
};
