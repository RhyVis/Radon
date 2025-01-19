<script lang="ts" setup>
import { FetchState, useVersionStore } from "@/store/version.ts";
import { get, useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const version = useVersionStore();
const { t } = useI18n();
const { fetchState, cCompileTimeL, cAssembleTimeL, cAssembleTimeR } = storeToRefs(version);
const [sw, setSw] = useToggle(true);

const tagTheme = computed(() => {
  switch (fetchState.value) {
    case FetchState.SUCCESS:
      return "success";
    case FetchState.NEED_UPDATE | FetchState.NOT_ONLINE:
      return "warning";
    case FetchState.ERROR:
      return "danger";
    default:
      return "default";
  }
});
const tagIcon = computed(() => {
  switch (fetchState.value) {
    case FetchState.SUCCESS:
      return "check-circle";
    case FetchState.NEED_UPDATE | FetchState.NOT_ONLINE:
      return "info-circle";
    case FetchState.ERROR:
      return "error-circle";
    default:
      return "help-circle";
  }
});
const tagValue = computed(() => {
  const prefix = t("clientVersion");
  switch (fetchState.value) {
    case FetchState.SUCCESS:
      return prefix + get(cAssembleTimeL);
    case FetchState.NEED_UPDATE:
      return prefix + get(cAssembleTimeL) + " -> " + get(cAssembleTimeR);
    case FetchState.ERROR:
      return prefix + t("display.error");
    case FetchState.NOT_ONLINE:
      return prefix + t("display.local");
    case FetchState.INIT:
      return prefix + get(cAssembleTimeL) + t("display.wait");
    default:
      return prefix + t("display.wait");
  }
});
const tagTimeValue = computed(() => t("clientCompile") + new Date(get(cCompileTimeL)).toLocaleString());
</script>

<template>
  <t-tag :theme="tagTheme" size="small" variant="outline">
    <template #icon>
      <t-icon :name="tagIcon" />
    </template>
    <span class="r-no-select" @click="setSw()">
      <Transition mode="out-in">
        <span v-if="sw">{{ tagValue }}</span>
        <span v-else>{{ tagTimeValue }}</span>
      </Transition>
    </span>
  </t-tag>
</template>

<style lang="less" scoped>
.v-enter-active,
.v-leave-active {
  transition: opacity 0.24s ease;
}

.v-enter-from,
.v-leave-to {
  opacity: 0;
}
</style>

<i18n lang="yaml" locale="en">
clientVersion: "Client Version: "
clientCompile: "Client Compile Time: "
display:
  error: "Error"
  wait: "Waiting"
  local: "(Local)"
msg:
  update: "Need to update"
  error: "Version fetch failed"
  comm-error: "Communication with server failed"
</i18n>

<i18n lang="yaml" locale="zh-CN">
clientVersion: "客户端版本: "
clientCompile: "客户端编译时间: "
display:
  error: "错误"
  wait: "等待中"
  local: "(本地)"
msg:
  update: "需要更新"
  error: "版本获取失败"
  comm-error: "与服务器通信失败"
</i18n>
