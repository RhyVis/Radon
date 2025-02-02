<script lang="tsx" setup>
import {
  authLogin,
  authLogout,
  authRefresh,
  authValidate,
  getAuthPassport,
  getAuthToken,
} from "@/lib/common/authMethods";
import { ArrowUpDown2Icon, Fingerprint2Icon, ImageAddIcon, LoginIcon, LogoutIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { useGlobalStore } from "@/store/global";
import { useRouter } from "vue-router";
import { useI18n } from "vue-i18n";
import { computed, onMounted, reactive, ref } from "vue";
import { get, set, useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { apiPostNoContent } from "@/lib/common/apiMethods.ts";
import { useSettingStore } from "@/store/settings.ts";

const { t } = useI18n();
const { authPassed } = storeToRefs(useGlobalStore());
const { showLoginMessage } = storeToRefs(useSettingStore());
const router = useRouter();
const query = reactive({
  username: "",
  password: "",
});
const imageTokenInput = ref("");

const [loading, setLoading] = useToggle(false);
const storageToken = computed(() => getAuthToken() || "");
const tokenValidSign = ref(0);
const tokenValidDisplay = computed(() => {
  switch (tokenValidSign.value) {
    case 0:
      return <t-icon name="lock-time" />;
    case 1:
      return <t-icon name="lock-off" />;
    default:
      return <t-icon name="lock-on" />;
  }
});

const handleLogin = async () => {
  if (query.username.length > 0 && query.password.length > 0) {
    setLoading(true);
    try {
      const b = await authLogin(query);
      if (b) {
        set(authPassed, true);
        setTimeout(() => router.push("/"), 2000);
        await MessagePlugin.success(t("msg.loginSuccess"));
      } else {
        await MessagePlugin.warning(t("msg.loginFailed"));
      }
    } catch (e) {
      console.error(e);
      await MessagePlugin.warning(t("msg.loginFailed"));
    } finally {
      setLoading(false);
    }
  } else {
    await MessagePlugin.warning(t("msg.noEmpty"));
  }
};
const handleLogout = async () => {
  setLoading(true);
  try {
    if (await authLogout()) {
      set(authPassed, false);
      setTimeout(() => router.push("/"), 2000);
      await MessagePlugin.success(t("msg.logoutSuccess"));
    } else {
      await MessagePlugin.warning(t("msg.logoutFailed"));
    }
  } catch (e) {
    console.error(e);
    await MessagePlugin.warning(t("msg.logoutFailed"));
  }
  setLoading(false);
};
const handleTokenStateCheck = async () => {
  tokenValidSign.value = 0;
  const b = await authValidate();
  if (b) {
    tokenValidSign.value = 1;
    await MessagePlugin.success(t("msg.tokenValid"));
  } else {
    tokenValidSign.value = 2;
    await MessagePlugin.warning(t("msg.tokenInvalid"));
  }
};
const handleRefreshToken = async () => {
  const b = await authRefresh();
  if (b) {
    await MessagePlugin.success(t("msg.refreshTokenSuccess"));
  } else {
    await MessagePlugin.warning(t("msg.refreshTokenFailed"));
  }
  setTimeout(() => location.reload(), 2000);
};

const handleAppendImageToken = async () => {
  if (get(imageTokenInput).length === 0) {
    await MessagePlugin.warning(t("msg.noEmpty"));
  } else {
    await apiPostNoContent("/api/auth/append/image-token", get(imageTokenInput));
    await MessagePlugin.success(t("msg.imageTokenAppend"));
  }
};

onMounted(() => {
  if (get(authPassed)) {
    const imageToken = getAuthPassport()?.extra.imageToken;
    if (imageToken) {
      set(imageTokenInput, imageToken);
    }
  }
});
</script>

<template>
  <content-layout :subtitle="t('st')" :title="t('tt')">
    <t-form @submit="handleLogin">
      <t-form-item :label="t('input.username')">
        <t-input v-model="query.username" />
      </t-form-item>
      <t-form-item :label="t('input.password')">
        <t-input v-model="query.password" type="password" />
      </t-form-item>
      <t-form-item :label="t('input.login')">
        <t-button :loading="loading" shape="round" theme="primary" type="submit">
          <LoginIcon v-if="!loading" />
        </t-button>
      </t-form-item>
      <template v-if="authPassed">
        <t-form-item :label="t('input.logout')">
          <t-button :loading="loading" shape="round" theme="warning" @click="handleLogout">
            <LogoutIcon v-if="!loading" />
          </t-button>
        </t-form-item>
        <t-divider />
        <t-form-item :label="t('input.tokenState')">
          <t-button shape="round" theme="default" @click="handleTokenStateCheck">
            <Component :is="tokenValidDisplay" />
          </t-button>
        </t-form-item>
        <t-form-item :label="t('input.refreshToken')">
          <t-button shape="round" theme="default" @click="handleRefreshToken">
            <ArrowUpDown2Icon />
          </t-button>
        </t-form-item>
        <t-form-item :label="t('input.showToken')">
          <t-popup :content="storageToken" :destroy-on-close="true" trigger="click">
            <t-button shape="round" theme="default">
              <Fingerprint2Icon />
            </t-button>
          </t-popup>
        </t-form-item>
        <t-divider />
        <t-form-item :label="t('input.imageToken')">
          <t-space>
            <t-input v-model="imageTokenInput" />
            <t-button shape="round" theme="default" @click="handleAppendImageToken">
              <ImageAddIcon />
            </t-button>
          </t-space>
        </t-form-item>
        <t-form-item :label="t('input.showLoginMessage')">
          <t-switch v-model="showLoginMessage" />
        </t-form-item>
      </template>
    </t-form>
  </content-layout>
</template>

<i18n locale="en">
tt: Authorization
st: You are in a mysterious page
input:
  username: Username
  password: Password
  login: Login
  logout: Logout
  tokenState: Token State
  refreshToken: Refresh Token
  showToken: Show Token
  clearToken: Clear Token
  imageToken: Image Token
  showLoginMessage: Show Login Message
msg:
  noEmpty: Do not leave input empty
  loginSuccess: Login Success
  loginFailed: Login Failed
  logoutSuccess: Logout Success
  logoutFailed: Logout Failed
  tokenValid: Token Valid
  tokenInvalid: Token Invalid
  refreshTokenSuccess: Refresh Token Success
  refreshTokenFailed: Refresh Token Failed
  tokenCleared: Token Cleared
  tokenConfirm: Confirm Clear Token?
  imageTokenAppend: Image Token Appended
</i18n>

<i18n locale="zh-CN">
tt: 验证
st: 你来到了神秘的页面
input:
  username: 用户名
  password: 密码
  login: 登陆
  logout: 登出
  tokenState: 令牌状态
  refreshToken: 刷新令牌
  showToken: 显示令牌
  clearToken: 清空令牌
  imageToken: 图片令牌
  showLoginMessage: 显示登陆消息
msg:
  noEmpty: 请不要留空
  loginSuccess: 登陆成功
  loginFailed: 登陆失败
  logoutSuccess: 登出成功
  logoutFailed: 登出失败
  tokenValid: 令牌有效
  tokenInvalid: 令牌无效
  refreshTokenSuccess: 刷新令牌成功
  refreshTokenFailed: 刷新令牌失败
  tokenCleared: 令牌已清空
  tokenConfirm: 确认清空令牌吗
  imageTokenAppend: 图片令牌已添加
</i18n>
