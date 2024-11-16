<script setup lang="tsx">
import { MessagePlugin, type TNodeReturnValue } from "tdesign-vue-next";
import { ArrowLeftIcon, RefreshIcon } from "tdesign-icons-vue-next";
import { decimalRadixValExtended } from "@/pages/math/radix/scripts/radix";
import VersionView from "@/assets/local/version.json";
import moment from "moment";
import { useStatic } from "@/lib/composable/useStatic";
import { useI18n } from "vue-i18n";
import { onMounted, ref, computed } from "vue";
import { getVersion } from "@/lib/common/apiMethods";

const loading = ref(true);
const { t } = useI18n();
const { fontFamily } = useStatic();

const versionFont = ref({
  fontWeight: "bold",
  fontFamily: fontFamily,
});

const vLocal = VersionView.compileTime;
const vRemote = ref(0);

const vState = ref(9);
const vDisplay = computed<{
  theme: "success" | "warning" | "danger" | "default" | "primary";
  icon: () => TNodeReturnValue;
  value: string;
}>(() => {
  switch (vState.value) {
    case 0:
      return {
        theme: "success",
        icon: () => <t-icon name="check-circle" />,
        value: `${decimalRadixValExtended(vLocal)} ${t("display.latest")}`,
      };
    case 1:
      return {
        theme: "warning",
        icon: () => <t-icon name="info-circle" />,
        value: `${decimalRadixValExtended(vLocal)} -> ${decimalRadixValExtended(vRemote.value)} ${t("display.update")}`,
      };
    case -1:
      return {
        theme: "danger",
        icon: () => <t-icon name="error-circle" />,
        value: t("display.error"),
      };
    default:
      return {
        theme: "default",
        icon: () => <t-icon name="help-circle" />,
        value: t("display.wait"),
      };
  }
});
const showDialog = ref(false);

const handleUpdate = async () => {
  if (vState.value != 0) {
    try {
      const f = await fetch("/index.html", { cache: "reload" });
      const text = await f.text();
      document.open();
      document.write(text);
      document.close();
    } catch (e) {
      console.error(e);
    } finally {
      showDialog.value = false;
    }
  }
};

onMounted(async () => {
  try {
    const v = await getVersion();
    if (v != 0) {
      vRemote.value = v;
      console.log(`vR: ${v} | vL: ${vLocal}`);
      if (v != vLocal) {
        vState.value = 1;
        showDialog.value = true;
        await MessagePlugin.warning(t("message.update"));
      } else {
        vState.value = 0;
      }
    } else {
      vState.value = -1;
      await MessagePlugin.warning(t("message.error"));
    }
  } catch (e) {
    vState.value = -1;
    console.error(e);
    await MessagePlugin.error(t("message.comm-error"));
  } finally {
    loading.value = false;
  }
});
</script>

<template>
  <t-card class="r-vc-card r-vc-card-override" :loading="loading">
    <t-space class="r-no-select" direction="vertical" :size="8">
      <div>
        <t-tag :theme="vDisplay.theme" :icon="vDisplay.icon" size="small">
          <span :style="versionFont">{{ vDisplay.value }}</span>
        </t-tag>
      </div>
      <div v-if="vState !== 0">
        <t-space>
          <t-button size="small" shape="circle" :theme="vDisplay.theme" @click="handleUpdate">
            <RefreshIcon />
          </t-button>
          <ArrowLeftIcon />
          <t-tag class="r-vc-refresh-font">{{ t("refresh") }}</t-tag>
        </t-space>
      </div>
    </t-space>
  </t-card>
  <t-dialog
    v-model:visible="showDialog"
    :header="t('dialog.title')"
    theme="warning"
    :confirm-btn="t('dialog.action')"
    :close-btn="false"
    @confirm="handleUpdate"
  >
    <t-space class="r-no-select" direction="vertical">
      <div>{{ t("dialog.content") }}：</div>
      <div>
        <span>{{ t("dialog.build") }}：</span>
        <t-tag :style="versionFont">{{ decimalRadixValExtended(vRemote) }}</t-tag>
      </div>
      <div>
        <span>{{ t("dialog.time") }}：</span>
        <t-tag :style="versionFont">{{ moment(vRemote).format("YYYY/MM/DD HH:mm:ss") }}</t-tag>
      </div>
      <div>{{ t("dialog.update") }}</div>
    </t-space>
  </t-dialog>
</template>

<style scoped lang="less">
.r-vc-card {
  width: max-content;
  .r-vc-refresh-font {
    font-weight: bold;
  }
}
.r-vc-card-override :deep(.t-card__body) {
  padding: 10px 14px 8px;
}
</style>

<i18n lang="yaml" locale="en">
display:
  latest: "Latest Version"
  update: "Update Required"
  error: "Version Fetch Failed"
  wait: "Waiting for Version Fetch"
message:
  update: "Non-latest version"
  error: "Version fetch failed"
  comm-error: "Communication with server failed"
refresh: "Click to refresh the page to update"
dialog:
  title: "Version Update"
  content: "New version detected"
  build: "Build Version"
  time: "Build Time"
  update: "Update?"
  action: "Update"
</i18n>

<i18n lang="yaml" locale="zh-CN">
display:
  latest: "最新版本"
  update: "需要更新"
  error: "版本获取失败"
  wait: "等待版本获取"
message:
  update: "非最新版本"
  error: "版本获取失败"
  comm-error: "与服务器通信失败"
refresh: "点击刷新页面以更新"
dialog:
  title: "版本更新"
  content: "检测到新版本"
  build: "构建版本"
  time: "构建时间"
  update: "更新？"
  action: "更新"
</i18n>
