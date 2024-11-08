<script setup lang="tsx">
import { authLogin, authRefresh, authValidate } from "@/lib/common/authMethods";
import { ArrowUpDown2Icon, DeleteIcon, Fingerprint2Icon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { useGlobalStore } from "@/store/global";

const { t } = useI18n();
const global = useGlobalStore();
const router = useRouter();
const query = reactive({
  username: "",
  password: "",
});

const storageToken = computed({
  get: () => localStorage.getItem("token") || "",
  set: (v: string) => localStorage.setItem("token", v),
});

const loginLoading = ref(false);
const tokenValidSign = ref(0);
const tokenValidDisplay = computed(() => {
  if (tokenValidSign.value === 0) {
    return <t-icon name="lock-time" />;
  } else if (tokenValidSign.value === 1) {
    return <t-icon name="lock-off" />;
  } else {
    return <t-icon name="lock-on" />;
  }
});

const handleLogin = async () => {
  if (query.username.length > 0 && query.password.length > 0) {
    loginLoading.value = true;
    const b = await authLogin(query);
    if (b) {
      global.authPassed = true;
      await MessagePlugin.success(t("msg.loginSuccess"));
    } else {
      await MessagePlugin.warning(t("msg.loginFailed"));
    }
    loginLoading.value = false;
    setTimeout(() => router.push("/"), 2000);
  } else {
    await MessagePlugin.warning(t("msg.noEmpty"));
  }
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
const handleClearToken = () => {
  storageToken.value = "";
  MessagePlugin.info(t("msg.tokenCleared"));
  setTimeout(() => location.reload(), 2000);
};
</script>

<template>
  <content-layout :title="t('tt')" :subtitle="t('st')">
    <t-form>
      <t-form-item :label="t('input.username')">
        <t-input v-model="query.username" />
      </t-form-item>
      <t-form-item :label="t('input.password')">
        <t-input v-model="query.password" type="password" />
      </t-form-item>
      <t-form-item :label="t('input.login')">
        <t-button shape="round" theme="primary" @click="handleLogin" :loading="loginLoading">
          <ArrowUpDown2Icon />
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
        <t-popup :content="storageToken" trigger="click" :destroy-on-close="true">
          <t-button shape="round" theme="default">
            <Fingerprint2Icon />
          </t-button>
        </t-popup>
      </t-form-item>
      <t-form-item :label="t('input.clearToken')">
        <t-popconfirm :content="t('msg.tokenConfirm')" theme="warning" @confirm="handleClearToken">
          <t-button shape="round" theme="default">
            <DeleteIcon />
          </t-button>
        </t-popconfirm>
      </t-form-item>
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
  tokenState: Token State
  refreshToken: Refresh Token
  showToken: Show Token
  clearToken: Clear Token
msg:
  noEmpty: Do not leave input empty
  loginSuccess: Login Success
  loginFailed: Login Failed
  tokenValid: Token Valid
  tokenInvalid: Token Invalid
  refreshTokenSuccess: Refresh Token Success
  refreshTokenFailed: Refresh Token Failed
  tokenCleared: Token Cleared
  tokenConfirm: Confirm Clear Token?
</i18n>

<i18n locale="zh-CN">
tt: 授权
st: 你来到了神秘的页面
input:
  username: 用户名
  password: 密码
  login: 登陆
  tokenState: 令牌状态
  refreshToken: 刷新令牌
  showToken: 显示令牌
  clearToken: 清空令牌
msg:
  noEmpty: 请不要留空
  loginSuccess: 登陆成功
  loginFailed: 登陆失败
  tokenValid: 令牌有效
  tokenInvalid: 令牌无效
  refreshTokenSuccess: 刷新令牌成功
  refreshTokenFailed: 刷新令牌失败
  tokenCleared: 令牌已清空
  tokenConfirm: 确认清空令牌吗
</i18n>
