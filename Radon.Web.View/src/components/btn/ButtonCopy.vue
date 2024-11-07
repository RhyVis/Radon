<script setup lang="ts">
import { CopyIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import useClipboard from "vue-clipboard3";

const props = defineProps({
  target: {
    type: String,
    default: "",
    required: true,
  },
});

const { t } = useI18n();
const { toClipboard } = useClipboard();

const action = async () => {
  const { target } = props;
  if (target.length > 0) {
    try {
      await toClipboard(target);
      await MessagePlugin.success(t("copySuccess"));
    } catch (e) {
      console.error(e);
      await MessagePlugin.error(t("copyFail"));
    }
  } else {
    await MessagePlugin.error(t("emptyContent"));
  }
};
</script>

<template>
  <t-tooltip :content="t('copy')" placement="top">
    <t-button theme="default" shape="square" @click="action">
      <CopyIcon />
    </t-button>
  </t-tooltip>
</template>

<i18n locale="en">
copy: "Copy content"
copySuccess: "Copy success"
copyFail: "Copy fail"
emptyContent: "Can't copy empty content"
</i18n>

<i18n locale="zh-CN">
copy: "复制内容"
copySuccess: "复制成功"
copyFail: "复制失败"
emptyContent: "不能复制空内容"
</i18n>
