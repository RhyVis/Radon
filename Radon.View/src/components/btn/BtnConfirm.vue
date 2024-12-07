<script lang="ts" setup>
import type { ButtonProps, PopconfirmProps } from "tdesign-vue-next";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const {
  theme = "danger",
  variant,
  shape,
  size,
  placement = "top",
  showArrow = true,
} = defineProps<{
  icon?: string;
  theme?: Exclude<ButtonProps["theme"], "primary" | "success">;
  variant?: ButtonProps["variant"];
  shape?: ButtonProps["shape"];
  size?: ButtonProps["size"];
  placement?: PopconfirmProps["placement"];
  showArrow?: PopconfirmProps["showArrow"];
}>();
const emit = defineEmits(["confirm", "cancel"]);
</script>

<template>
  <t-popconfirm
    :cancel-btn="{
      theme: 'default',
      content: t('no'),
    }"
    :confirm-btn="{
      theme: theme,
      content: t('yes'),
    }"
    :content="t('confirm')"
    :placement="placement"
    :show-arrow="showArrow"
    :theme="theme"
    @cancel="emit('cancel')"
    @confirm="emit('confirm')"
  >
    <t-button :shape="shape" :size="size" :theme="theme" :variant="variant">
      <t-icon v-if="icon" :name="icon" />
      <slot />
    </t-button>
  </t-popconfirm>
</template>

<i18n locale="en">
confirm: Confirm Operation?
yes: Yes
no: No
</i18n>

<i18n locale="zh-CN">
confirm: 确认操作？
yes: 是
no: 否
</i18n>
