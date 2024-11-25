import { authValidate } from "@/lib/common/authMethods";
import i18n from "@/locale";
import { get, useOnline } from "@vueuse/core";
import { MessagePlugin } from "tdesign-vue-next";
import NotificationPlugin from "tdesign-vue-next/es/notification/plugin";
import type { NavigationGuardNext, RouteLocationNormalizedGeneric, RouteLocationNormalizedLoaded } from "vue-router";

const { t } = i18n.global;

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
      next(false);
    }
  } else {
    next();
  }
};

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

export const saveConfirmGuard = async (
  _: RouteLocationNormalizedGeneric,
  from: RouteLocationNormalizedLoaded,
  next: NavigationGuardNext,
) => {
  if (from.meta.saveConfirm) {
    next(
      await new Promise<boolean>(resolve => {
        NotificationPlugin.warning({
          title: t("message.saveConfirm.title"),
          content: t("message.saveConfirm.content"),
          duration: 10000,
          closeBtn: t("message.saveConfirm.confirm"),
          onCloseBtnClick: () => {
            resolve(true);
          },
          onDurationEnd: () => {
            resolve(false);
          },
        });
      }),
    );
  } else {
    next();
  }
};
