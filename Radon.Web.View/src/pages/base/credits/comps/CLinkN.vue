<script setup lang="tsx">
const linkIcon = () => <t-icon name="jump" />;

defineProps<{
  href: string;
}>();
</script>

<template>
  <t-link theme="default" :href="href" :suffix-icon="linkIcon">
    <slot />
  </t-link>
</template>
