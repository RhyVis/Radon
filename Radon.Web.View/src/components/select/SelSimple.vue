<script setup lang="ts">
type SimpleSelOpt = {
  label?: string;
  value?: string;
};

const selected = defineModel<string>("selected", { required: true });
const emit = defineEmits(["update"]);
defineProps<{
  options: SimpleSelOpt[];
  placeholder?: string;
  loading?: boolean;
  autoWidth?: boolean;
}>();

const { t } = useI18n();
</script>

<template>
  <t-select
    v-model="selected"
    :placeholder="placeholder ?? t('selSimple.placeholder')"
    :auto-width="autoWidth"
    :loading="loading ?? false"
    :loading-text="t('selSimple.loading')"
    @change="emit('update', selected)"
  >
    <t-option v-for="option in options" :key="option.value" :value="option.value" :label="option.label" />
  </t-select>
</template>

<i18n lang="yaml" locale="en">
selSimple:
  placeholder: Please select
  loading: Loading
</i18n>

<i18n lang="yaml" locale="zh-CN">
selSimple:
  placeholder: 请选择
  loading: 加载中
</i18n>
