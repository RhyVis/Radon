<script setup lang="ts">
import { useVersionStore } from "@/store/version.ts";
import { get, useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { MessagePlugin } from "tdesign-vue-next";
import { useRegisterSW } from "virtual:pwa-register/vue";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const { cAssembleTimeR, cCompileTimeRFormatted } = storeToRefs(useVersionStore());
const { needRefresh, updateServiceWorker } = useRegisterSW({
  immediate: true,
  onRegisteredSW(_, r) {
    if (r)
      setInterval(async () => {
        await r.update();
      }, 30000);
  },
});
const [dialogPass, setDialogPass] = useToggle(true);
const dialog = computed({
  get: () => get(dialogPass) && get(needRefresh),
  set: v => {
    if (!v) handleClose();
  },
});
const buildVersion = computed(() => get(cAssembleTimeR));
const handleClose = () => {
  setDialogPass(false);
  MessagePlugin.info(t("later"));
};
</script>

<template>
  <t-dialog
    v-model:visible="dialog"
    :header="t('tt')"
    placement="top"
    :confirm-btn="t('btn.confirm')"
    :cancel-btn="t('btn.cancel')"
    :close-btn="false"
    width="75%"
    @confirm="updateServiceWorker()"
    @close="handleClose"
  >
    <t-space direction="vertical">
      <t-text class="r-no-select">
        {{ t("newReady") }}
      </t-text>
      <t-text class="r-no-select">
        {{ t("buildVersion") }}
        <t-tag>{{ buildVersion }}</t-tag>
      </t-text>
      <t-text class="r-no-select">
        {{ t("buildTime") }}
        <t-tag>{{ cCompileTimeRFormatted }}</t-tag>
      </t-text>
    </t-space>
  </t-dialog>
</template>

<i18n locale="en">
tt: Update available
newReady: "New version content is ready"
buildVersion: "Build version: "
buildTime: "Build time: "
btn: 
  confirm: Update
  cancel: Later
later: Client will update next time when it is opened
</i18n>

<i18n locale="zh-CN">
tt: 可更新
newReady: "新版本内容已就绪"
buildVersion: "构建版本: "
buildTime: "构建时间: "
btn: 
  confirm: 更新
  cancel: 稍后
later: 客户端将在下次打开时更新
</i18n>
