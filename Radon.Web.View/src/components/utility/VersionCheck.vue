<script setup lang="tsx">
import { computed, onMounted, ref } from "vue";
import { getVersion } from "@/lib/util/apiMethods";
import { MessagePlugin } from "tdesign-vue-next";
import { ArrowLeftIcon, RefreshIcon } from "tdesign-icons-vue-next";
import { decimalRadixValExtended } from "@/pages/math/radix/scripts/radix";
import VersionView from "@/assets/local/version.json";
import moment from "moment";
import useStatic from "@/lib/util/useStatic";
import { useI18n } from "vue-i18n";

const loading = ref(true);
const { t } = useI18n();

const versionFont = ref({
  fontWeight: "bold",
  fontFamily: useStatic().fontFamily,
});

const vLocal = VersionView.compileTime;
const vRemote = ref(0);

const vState = ref(9);
const vDisplay = computed(() => {
  switch (vState.value) {
    case 0:
      return {
        theme: "success",
        icon: () => <t-icon name="check-circle" />,
        value: `${decimalRadixValExtended(vLocal)} ${t("versionCheck.display.latest")}`,
      };
    case 1:
      return {
        theme: "warning",
        icon: () => <t-icon name="info-circle" />,
        value: `${decimalRadixValExtended(vLocal)} -> ${decimalRadixValExtended(vRemote.value)} ${t("versionCheck.display.update")}`,
      };
    case -1:
      return {
        theme: "danger",
        icon: () => <t-icon name="error-circle" />,
        value: t("versionCheck.display.error"),
      };
    default:
      return {
        theme: "default",
        icon: () => <t-icon name="help-circle" />,
        value: t("versionCheck.display.wait"),
      };
  }
});
const showDialog = ref(false);

const handleUpdate = () => {
  if (vState.value != 0) {
    //    fetch(window.location.host, { cache: "no-store" })
    //      .then(response => response.text())
    //      .then(html => {
    //        document.open();
    //        document.write(html);
    //        document.close();
    //      });
    window.location.reload();
  }
  showDialog.value = false;
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
        await MessagePlugin.warning(t("versionCheck.message.update"));
      } else {
        vState.value = 0;
      }
    } else {
      vState.value = -1;
      await MessagePlugin.warning(t("versionCheck.message.error"));
    }
  } catch (e) {
    vState.value = -1;
    console.error(e);
    await MessagePlugin.error(t("versionCheck.message.comm-error"));
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
          <t-tag class="r-vc-refresh-font">{{ t("versionCheck.refresh") }}</t-tag>
        </t-space>
      </div>
    </t-space>
  </t-card>
  <t-dialog
    v-model:visible="showDialog"
    header="版本更新"
    theme="warning"
    confirm-btn="更新"
    :close-btn="false"
    @confirm="handleUpdate"
  >
    <t-space class="r-no-select" direction="vertical">
      <div>检查到新版本：</div>
      <div>
        <span>构建版本：</span>
        <t-tag :style="versionFont">{{ decimalRadixValExtended(vRemote) }}</t-tag>
      </div>
      <div>
        <span>构建时间：</span>
        <t-tag :style="versionFont">{{ moment(vRemote).format("YYYY/MM/DD HH:mm:ss") }}</t-tag>
      </div>
      <div>是否更新？</div>
    </t-space>
  </t-dialog>
</template>

<style scoped>
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

<i18n lang="yaml">
en:
  versionCheck:
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
zh-CN:
  versionCheck:
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
</i18n>
