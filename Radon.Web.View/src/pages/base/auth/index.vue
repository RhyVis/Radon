<script setup lang="tsx">
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import { login, refresh, validate } from "@/lib/util/authMethods";
import { ArrowUpDown2Icon, DeleteIcon, Fingerprint2Icon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, reactive, ref } from "vue";
import { useRouter } from "vue-router";
import { useGlobalStore } from "@/store/global";

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
    const b = await login(query);
    if (b) {
      global.authPassed = true;
      await MessagePlugin.success("登陆成功");
    } else {
      await MessagePlugin.warning("登陆失败");
    }
    loginLoading.value = false;
    setTimeout(() => router.push("/"), 2000);
  } else {
    await MessagePlugin.warning("不要空置输入框");
  }
};
const handleTokenStateCheck = async () => {
  tokenValidSign.value = 0;
  const b = await validate();
  if (b) {
    tokenValidSign.value = 1;
    await MessagePlugin.success("令牌有效");
  } else {
    tokenValidSign.value = 2;
    await MessagePlugin.warning("令牌无效");
  }
};
const handleRefreshToken = async () => {
  const b = await refresh();
  if (b) {
    await MessagePlugin.success("令牌刷新成功");
  } else {
    await MessagePlugin.warning("令牌刷新失败");
  }
  setTimeout(() => location.reload(), 2000);
};
const handleClearToken = () => {
  storageToken.value = "";
  MessagePlugin.info("令牌已清空");
  setTimeout(() => location.reload(), 2000);
};
</script>

<template>
  <ContentLayout title="校验" subtitle="你来到了神秘的页面">
    <t-form>
      <t-form-item label="用户名">
        <t-input v-model="query.username" />
      </t-form-item>
      <t-form-item label="密码">
        <t-input v-model="query.password" type="password" />
      </t-form-item>
      <t-form-item label="登陆">
        <t-button shape="round" theme="primary" @click="handleLogin" :loading="loginLoading">
          <ArrowUpDown2Icon />
        </t-button>
      </t-form-item>
      <t-divider />
      <t-form-item label="令牌状态">
        <t-button shape="round" theme="default" @click="handleTokenStateCheck">
          <Component :is="tokenValidDisplay" />
        </t-button>
      </t-form-item>
      <t-form-item label="刷新令牌">
        <t-button shape="round" theme="default" @click="handleRefreshToken">
          <ArrowUpDown2Icon />
        </t-button>
      </t-form-item>
      <t-form-item label="显示令牌">
        <t-popup :content="storageToken" trigger="click" :destroy-on-close="true">
          <t-button shape="round" theme="default">
            <Fingerprint2Icon />
          </t-button>
        </t-popup>
      </t-form-item>
      <t-form-item label="清空令牌">
        <t-popconfirm content="确认清空令牌吗" theme="warning" @confirm="handleClearToken">
          <t-button shape="round" theme="default">
            <DeleteIcon />
          </t-button>
        </t-popconfirm>
      </t-form-item>
    </t-form>
  </ContentLayout>
</template>
