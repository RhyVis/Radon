<script setup lang="ts">
import { useGlobalStore } from "@/store/global";
import { get, set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { useRegisterSW } from "virtual:pwa-register/vue";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const { vRemoteShort } = storeToRefs(useGlobalStore());
const { offlineReady, needRefresh, updateServiceWorker } = useRegisterSW({
  immediate: true,
  onRegisteredSW(_, r) {
    if (r)
      setInterval(async () => {
        await r.update();
      }, 30000);
  },
});
const dialog = computed({
  get: () => get(needRefresh),
  set: v => {
    if (!v) handleClose();
  },
});
const buildVersion = computed(() =>
  t("buildVersion", { v: `${import.meta.env.PACKAGE_VERSION}.${get(vRemoteShort)}` }),
);
const handleClose = () => {
  set(offlineReady, false);
  set(needRefresh, false);
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
    @overlay-click="() => {}"
  >
    <span class="r-no-select">{{ t("newReady", { v: vRemoteShort }) }}</span>
    <span class="r-no-select">{{ buildVersion }}</span>
  </t-dialog>
</template>

<i18n locale="en">
tt: Update available
newReady: "New version content is ready: {v}"
buildVersion: "Build version: {v}"
btn: 
  confirm: Update
  cancel: Later
</i18n>

<i18n locale="zh-CN">
tt: 可更新
newReady: "新版本内容已就绪: {v}"
buildVersion: "构建版本: {v}"
btn: 
  confirm: 更新
  cancel: 稍后
</i18n>
