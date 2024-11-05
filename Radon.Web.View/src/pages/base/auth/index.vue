<script setup lang="ts">
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import { login } from "@/lib/util/authMethods";
import { useAuthStore } from "@/pages/base/auth/scripts/store";
import { ArrowUpDown2Icon, DeleteIcon, Fingerprint2Icon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, reactive, ref } from "vue";

const auth = useAuthStore();
const query = reactive({
  username: "",
  password: "",
});

const token = ref("");
const btnTheme = ref("primary");
const txtTheme = ref("default");
const storageToken = computed({
  get: () => localStorage.getItem("token") || "",
  set: (v: string) => {
    localStorage.setItem("token", v);
    token.value = v;
  },
});

const loginLoading = ref(false);

const handleLogin = async () => {
  if (query.username.length > 0 && query.password.length > 0) {
    loginLoading.value = true;
    const b = await login(query);
    if (b) {
      await MessagePlugin.success("登陆成功");
    } else {
      await MessagePlugin.warning("登陆失败");
    }
    loginLoading.value = false;
  } else {
    await MessagePlugin.warning("不要空置输入框");
  }
};
const handleValidation = async () => {
  btnTheme.value = "primary";
  txtTheme.value = "default";
  if (token.value.length > 0) {
    try {
      auth.updateToken(token.value);
      const b = await auth.validate;
      if (b) {
        await MessagePlugin.success("验证成功");
        btnTheme.value = "success";
        txtTheme.value = "success";
      } else {
        await MessagePlugin.warning("验证失败");
        btnTheme.value = "warning";
        txtTheme.value = "warning";
      }
    } catch (e) {
      console.error(e);
      await MessagePlugin.error("更新验证状况失败");
      btnTheme.value = "danger";
    }
  } else {
    await MessagePlugin.warning("不要空置输入框");
    btnTheme.value = "warning";
    txtTheme.value = "warning";
  }
};
</script>

<template>
  <ContentLayout title="校验" subtitle="你来到了神秘的页面">
    <t-form>
      <t-form-item label="当前状态">
        <t-tag class="r-no-select" v-if="auth.valid" shape="round" theme="success" content="通过" />
        <t-tag class="r-no-select" v-else shape="round" theme="warning" content="失败" />
      </t-form-item>
      <t-form-item label="校验码">
        <t-input v-model="token" />
      </t-form-item>
      <t-form-item label="用户名">
        <t-input v-model="query.username" />
      </t-form-item>
      <t-form-item label="密码">
        <t-input v-model="query.password" type="password" />
      </t-form-item>
      <t-form-item label="验证">
        <t-button shape="round" :theme="btnTheme" @click="handleValidation">
          <ArrowUpDown2Icon />
        </t-button>
      </t-form-item>
      <t-form-item label="登陆">
        <t-button shape="round" theme="primary" @click="handleLogin" :loading="loginLoading">
          <ArrowUpDown2Icon />
        </t-button>
      </t-form-item>
      <t-divider />
      <t-form-item label="显示令牌">
        <t-popup :content="storageToken" trigger="click" :destroy-on-close="true">
          <t-button shape="round" theme="default">
            <Fingerprint2Icon />
          </t-button>
        </t-popup>
      </t-form-item>
      <t-form-item label="清空令牌">
        <t-popconfirm content="确认清空令牌吗" theme="warning" @confirm="storageToken = ''">
          <t-button shape="round" theme="default">
            <DeleteIcon />
          </t-button>
        </t-popconfirm>
      </t-form-item>
    </t-form>
  </ContentLayout>
</template>

<style scoped>
.r-no-select {
  user-select: none;
}
</style>
