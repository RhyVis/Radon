<script setup lang="ts">
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import { useAuthStore } from "@/store/comps/auth";
import { ArrowUpDown2Icon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { ref } from "vue";

const auth = useAuthStore();

const token = ref("");
const btnTheme = ref("primary");
const txtTheme = ref("default");

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
      <t-form-item label="验证">
        <t-button shape="round" :theme="btnTheme" @click="handleValidation">
          <ArrowUpDown2Icon />
        </t-button>
      </t-form-item>
    </t-form>
  </ContentLayout>
</template>

<style scoped>
.r-no-select {
  user-select: none;
}
</style>
